# ������� ���������� CafeHub

**������� ���������� CafeHub** - ��� ���������, ��������������� ��� ���������� � ������� ����� �������������, ������������ ��������������� ��������� ������. ��� ������� �������� � ���� ���������� ��� �������� �������, ��������� � ������� ���������.

## ����������

- [������ ������](#getting-started)
- [����������](#components)
  - [ReportController](#reportcontroller)
  - [BackgroundAnalyticsProcessor](#backgroundanalyticsprocessor)
  - [TextAnalyzer](#textanalyzer)
  - [DataAnalyzer](#dataanalyzer)
- [�������������](#usage)
- [����������](#contributing)
- [��������](#license)

## ������ ������

����� ������ ������ � �������� ���������� CafeHub, �������� ����������� ����.

### ��������������� ����������

���������, ��� � ��� ����������� ��������� ����������:

- .NET Core SDK
- Entity Framework Core
- Microsoft.AspNetCore

### ���������

1. ���������� �����������:

   ```bash
   git clone https://github.com/yourusername/CafeHubReportingSystem.git
   ```

2. ��������� � ������� �������:

   ```bash
   cd CafeHubReportingSystem
   ```

3. �������� � ��������� ����������:

   ```bash
   dotnet build
   dotnet run
   ```

## ����������

### ReportController

`ReportController` ������������ HTTP-�������, ��������� � ��������� �������, ���������� � ��������������� � �������������.

#### �������� �����

- `POST /report/doAnalytics`: �������� ������� ������������ �� ������.
- `GET /report/reports`: ��������� ������ �������, ��������� � ������������������� �������������.
- `GET /report/reportId/{idReport}`: ��������� ��������� ���������� � ���������� ������ �� ID.
- `DELETE /report/deleteReport/{idReport}`: �������� ������ �� ID.

### BackgroundAnalyticsProcessor

`BackgroundAnalyticsProcessor` - ��� ������� ������, ������������� �� ����������� ��������� ���������. �� ���������� ������� ��� ��������� �������� ������������� � �������� �������.

### TextAnalyzer

����� `TextAnalyzer` ����������� ����������� � ���� ��������� ���������. �� ���������� ������ ��������� �������� ��� ������� ����������� � ������������ ��������� ����� ��� ����������� ����� � �������� ������ ����.

### DataAnalyzer

����� `DataAnalyzer` ������������ ������ ����� �������������. �� ��������� �������� ����������� ���������� �������, ���������� �� � ���������� �������� ����� `ReportSummary`.
