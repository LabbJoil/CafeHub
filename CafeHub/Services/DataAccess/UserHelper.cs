
using CafeHub.Models.Domain;
using CafeHub.Models.Entities;
using CafeHub.Models.Enums;
using CafeHub.Models.ReportProcessing;
using CafeHub.Services.Entities;
using CafeHub.Services.Helpers;
using CafeHub.Services.System;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CafeHub.Services.DataAccess;

public class UserHelper(ContextDB contextDB)
{
    private readonly ContextDB Database = contextDB;

    public User AuthorizeUsers(string email, string password)
    {
        User? user = Database.Users.FirstOrDefault(
            nextUser => nextUser.Email == email &&
            nextUser.Password == PasswordHelper.HashPassword(password ?? "")
            ) ?? throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, "You don't have account"));
        user.Password = "";
        return user;
    }

    public async Task<User> SetUser(User user)
    {
        if (Database.Users.FirstOrDefault(user => user.Email == user.Email) != null)
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, $"User with Login ({user.Email}) already exists"));
        User userModel = new(user);
        Database.Users.Add(userModel);
        await Database.SaveChangesAsync();
        userModel.Password = "";
        return userModel;
    }

    public User GetUserInfo(int idUser)
    {
        User user = Database.Users.Find(idUser) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, $"User with id ({idUser}) not found"));
        user.Password = "";
        return user;
    }

    public async Task<User> UpdateUser(int idUser, UpdateUserModel userModel)
    {
        User updateUser = Database.Users.Find(idUser) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, $"User with id ({idUser}) not found"));
        updateUser.UpdateUser(userModel);
        await Database.SaveChangesAsync();
        updateUser.Password = "";
        return updateUser;
    }

    public async Task UpdatePassword(int idUser, UpdatePasswordModel passwordModel)
    {
        User updateUser = Database.Users.Find(idUser) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, $"User with id ({idUser}) not found"));
        if (updateUser.Password == PasswordHelper.HashPassword(passwordModel.OldPassword ?? ""))
        {
            updateUser.UpdatePassword(passwordModel.NewPassword);
            await Database.SaveChangesAsync();
        }
        else throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, "The old password does not match"));
    }

    public async Task UpdateIcon(int idUser, byte[] iconBytes)
    {
        User updateUser = Database.Users.Find(idUser) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, $"User with id ({idUser}) not found"));
        updateUser.Icon = iconBytes;
        await Database.SaveChangesAsync();
    }

    public async Task DeleteUser(int idUser)
    {
        User deleteUser = Database.Users.Find(idUser) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, $"User with id ({idUser}) not found"));
        Database.Users.Remove(deleteUser);
        await Database.SaveChangesAsync();
    }

    public async Task<List<ComplaintSummary>> GetComplaints(UserRole role, int idUser)
    {
        List<Complaint> complaints = role == UserRole.Admin ? await Database.Complaints.ToListAsync() : await Database.Complaints.Where(compl => compl.IdUser == idUser).ToListAsync();
        List<ComplaintSummary> complaintSummaries = complaints.Select(complaint => new ComplaintSummary()
        {
            MainComplaint = complaint,
            UserComplain = Database.Users.Find(idUser),
            ListAttachmets = [.. Database.Attachments.Where(attachment => attachment.IdComplaint == complaint.Id)]
        }).ToList();
        return complaintSummaries;
    }

    public async Task<ComplaintSummary> GetComplaintById(UserRole role, int idComplaint, int idUser = 0)
    {
        Complaint complaint = (role == UserRole.Admin ? await Database.Complaints.FindAsync(idComplaint) : await Database.Complaints.FirstOrDefaultAsync(compl => compl.Id ==  idComplaint && compl.IdUser == idUser)) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, "The complaint is missing"));
        ComplaintSummary analyticReport = new()
        {
            MainComplaint = complaint,
            UserComplain = Database.Users.Where(user => user.Id == complaint.IdUser).Select(user => new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone
            }).FirstOrDefault(),
            ListAttachmets = [.. Database.Attachments.Where(compl => compl.IdComplaint == idComplaint)]
        };
        return analyticReport;
    }

    public async Task SetComplaint(int idUser, ComplaintSummary complaintSummary)
    {
        User user = Database.Users.Find(idUser) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, $"User with id ({idUser}) not found"));
        if (complaintSummary.MainComplaint != null)
        {
            complaintSummary.MainComplaint.IdUser = idUser;
            var addedComplaint = Database.Complaints.Add(complaintSummary.MainComplaint);
            await Database.SaveChangesAsync();
            int idComplaint = addedComplaint.Entity.Id;
            foreach (var attachmets in complaintSummary.ListAttachmets ?? [])
            {
                attachmets.IdComplaint = idComplaint;
                Database.Attachments.Add(attachmets);
            }
            await Database.SaveChangesAsync();
        }
    }

    public async Task DeleteComplaint(UserRole role, int idUser, int idComplaint)
    {
        Complaint deleteComplaint = (role == UserRole.Admin ? await Database.Complaints.FindAsync(idComplaint) : await Database.Complaints.FirstOrDefaultAsync(compl => compl.Id == idComplaint && compl.IdUser == idUser)) ??
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, "The complaint is missing"));
        Database.Complaints.Remove(deleteComplaint);
        await Database.SaveChangesAsync();
    }

    public async Task EditStatusComplaint(int idComplaint, StatusComplaint newStatus)
    {
        Complaint editStatusComplaint = await Database.Complaints.FirstOrDefaultAsync(compl => compl.Id == idComplaint) ?? 
            throw new ExceptionHelper(new LoggingHelper(TypeMessage.Problem, DangerLevel.Wanted, "The complaint is missing"));
        editStatusComplaint.Status = newStatus;
        await Database.SaveChangesAsync();
    }
}
