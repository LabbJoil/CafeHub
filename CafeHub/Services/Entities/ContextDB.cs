using Microsoft.EntityFrameworkCore;
using CafeHub.Models.Domain;

namespace CafeHub.Services.Entities;

public class ContextDB(DbContextOptions<ContextDB> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<CommonWord> CommonWords => Set<CommonWord>();
    public DbSet<Tonality> Tonalitys => Set<Tonality>();
    public DbSet<PartsSpeech> PartsSpeechs => Set<PartsSpeech>();
}
