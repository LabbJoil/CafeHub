# Система отчетности CafeHub

**Система отчетности CafeHub** - это платформа, предназначенная для управления и анализа жалоб пользователей, предоставляя администраторам подробные отчеты. Эта система включает в себя компоненты для создания отчетов, аналитики и фоновой обработки.

## Содержание

- [Начало работы](#getting-started)
- [Компоненты](#components)
  - [ReportController](#reportcontroller)
  - [BackgroundAnalyticsProcessor](#backgroundanalyticsprocessor)
  - [TextAnalyzer](#textanalyzer)
  - [DataAnalyzer](#dataanalyzer)
- [Использование](#usage)
- [Содействие](#contributing)
- [Лицензия](#license)

## Начало работы

Чтобы начать работу с системой отчетности CafeHub, следуйте инструкциям ниже.

### Предварительные требования

Убедитесь, что у вас установлены следующие компоненты:

- .NET Core SDK
- Entity Framework Core
- Microsoft.AspNetCore

### Установка

1. Клонируйте репозиторий:

   ```bash
   git clone https://github.com/yourusername/CafeHubReportingSystem.git
   ```

2. Перейдите в каталог проекта:

   ```bash
   cd CafeHubReportingSystem
   ```

3. Соберите и запустите приложение:

   ```bash
   dotnet build
   dotnet run
   ```

## Компоненты

### ReportController

`ReportController` обрабатывает HTTP-запросы, связанные с созданием отчетов, аналитикой и взаимодействием с пользователем.

#### Конечные точки

- `POST /report/doAnalytics`: Отправка диалога пользователя на анализ.
- `GET /report/reports`: Получение списка отчетов, связанных с аутентифицированным пользователем.
- `GET /report/reportId/{idReport}`: Получение подробной информации о конкретном отчете по ID.
- `DELETE /report/deleteReport/{idReport}`: Удаление отчета по ID.

### BackgroundAnalyticsProcessor

`BackgroundAnalyticsProcessor` - это фоновый сервис, ответственный за асинхронную обработку аналитики. Он использует очередь для обработки диалогов пользователей и создания отчетов.

### TextAnalyzer

Класс `TextAnalyzer` анализирует тональность и язык текстовых сообщений. Он использует модели машинного обучения для анализа тональности и естественную обработку языка для определения языка и разметки частей речи.

### DataAnalyzer

Класс `DataAnalyzer` оркестрирует анализ жалоб пользователей. Он управляет очередью результатов текстового анализа, объединяет их и генерирует обширный отчет `ReportSummary`.
