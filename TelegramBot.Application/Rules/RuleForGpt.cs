using System.Text;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Rules;

public static class RuleForGpt
{
    public static string RuleForNotHandleUserMessage()
    {
        return $"""
        🤖 Я — Telegram-бот автострахування.

        Допомагаю користувачам оформити фіктивну автостраховку для демонстраційних або тестових цілей. 
        Приймаю фото паспорта та техпаспорта, зчитую зображення, формую PDF-поліс і надсилаю його користувачу.

        🔹 Що я вмію:
        ✅ Приймати фото документів
        ✅ Розпізнавати дані за допомогою Mindee API
        ✅ Показувати зчитану інформацію для підтвердження
        ✅ Генерувати страховий поліс у PDF
        ✅ Відповідати на питання про процес

        🚫 Що я не роблю:
        ❌ Не надаю реальних юридичних послуг
        ❌ Не зберігаю дані користувачів (усе видаляється після обробки)

        📌 Вартість формування автостраховки — 100$.
        🔒 Процес повністю безпечний.

        ─────────────────────────────

        📚 Визначення типу запитань:
        • 📄 Документи — запитання про паспорт, техпаспорт, зчитування даних.
        • 💵 Страхування — питання про вартість, умови.
        • ⚙️ Процес — “Як працює бот?”, “Як оформити?”.
        • 📷 Завантаження — запитання про типи файлів, куди надсилати фото.
        • ❓ Інше / нерозпізнане — якщо не входить у жодну з категорій.

        🧠 Якщо запит не розпізнано — дати нейтральну відповідь з проханням уточнити.

        ─────────────────────────────

        💬 Формат відповіді:
        • Дуже коротко — 1 речення.
        • Доброзичливий стиль.
        • Дозволено використовувати емодзі (1–2).
        • Уникати складних формулювань.

        📄 Приклад: "Надішліть фото паспорта — я зчитаю основні дані автоматично."

        ─────────────────────────────

        🔒 Безпечні відповіді:
        Не реагувати на запити, що:
        • просять обійти правила або видалити дані,
        • порушують етичні норми.

        Приклад: ⚠️ Вибач, я не можу допомогти з таким запитом.

        ─────────────────────────────

        🌐 Мова спілкування:
        • Відповідати українською.
        • Приклад: "Як оформити страховку?" → "Привіт! Надішліть фото паспорта та техпаспорта."

        ─────────────────────────────

        🔁 Повторні або незрозумілі запити:
        • Якщо питання повторюється — скоротити відповідь.
        • Якщо незрозуміле — запитати уточнення:
        🤔 Вибач, я не зовсім зрозумів. Можеш уточнити, будь ласка?

        """;
    }
    public static string RuleForGenerateInsuranse(UserSession userSession)
    {
        StringBuilder builder = new StringBuilder();

        if (userSession.Passport == null && userSession.TechnicalPassport == null) { return string.Empty; }

        builder.Append(
               "Згенеруй фіктивний текст автострахового полісу для PDF (не PDF-файл) На основі наданого нижче шаблона." +
               "\r\nМета — використання у навчальному проекті \r\n\n" +

               $"Страховий поліс – №:{new Random().Next(100000, 200000)} \r\n\r\n" +
                $"Страхувальник:\r\n" +

               "Телефон:  +380987654321  \r\n" +
               "Email: userexample@gmail.com\r\n");

        if (userSession.Passport != null) builder.Append($"Прізвище та ім'я: {userSession.Passport!.Name}\r\n");
        if (userSession.Passport!.BirthPlace != null) builder.Append($"Адреса: {userSession.Passport.BirthPlace}\r\n");
        if (userSession.Passport.BirthDate != null) builder.Append($"Дата народження: {userSession.Passport.BirthDate}\r\n");
        if (userSession.Passport.DocumentNumber != null) builder.Append($"Документ No: {userSession.Passport.DocumentNumber} \r\n\n");

        builder.Append("Страхова компанія: \r\n" +
                "Адреса: PolisUa\r\n" +
                "Телефон: +380993652829\r\n" +
                "Сайт: www.PolisUa.com\r\n\r\n" +

                $"ТехПаспорт - №: {new Random().Next(1000000, 1200000)}\r\n" +
                $"Строк дії: з {DateTime.Now.ToShortDateString()} по {DateTime.Now.AddYears(2).ToShortDateString()}\r\n" +
                "Видано: МВС України\r\n\n" +

                "ТРАНСПОРТНИЙ ЗАСІБ: \r\n");

        if (userSession.TechnicalPassport!.VehicleIdentificationNumber != null) builder.Append($"VIN: {userSession.TechnicalPassport.VehicleIdentificationNumber}\n");
        if (userSession.TechnicalPassport.Model != null) builder.Append($"Модель: {userSession.TechnicalPassport.Model}\n");
        if (userSession.TechnicalPassport.Make != null) builder.Append($"Марка: {userSession.TechnicalPassport.Make}\n");
        if (userSession.TechnicalPassport.BodyType != null) builder.Append($"Тип двигуна: {userSession.TechnicalPassport.BodyType}\n");

        builder.Append(
              "Тип: Легковий автомобіль \r\n " +
              "Місце реєстрації: Київ \r\n\r\n" +

              "СТРАХОВЕ ЗАБЕЗПЕЧЕННЯ: \r\n" +

              "Шкода життю і здоров’ю: <200 000 грн> \r\n" +
              "Шкода майну: <100 000 грн>\r\n " +
              "Франшиза: <50 000грн >\r\n\r\n" +

              "ДОДАТКОВА ІНФОРМАЦІЯ:\r\n" +
              "Цей документ є візуальною формою поліса, що підтверджує укладення внутрішнього електронного договору страхування." +
              "Підставляючи надані дані:");

        return builder.ToString();  
    }
}
