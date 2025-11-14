using System.Text.Json;

const string FileName = "notes.json";

List<Note> LoadNotes()
{
    if (!File.Exists(FileName))
        return new List<Note>();

    var json = File.ReadAllText(FileName);
    return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
}

void SaveNotes(List<Note> notes)
{
    var json = JsonSerializer.Serialize(notes, new JsonSerializerOptions
    {
        WriteIndented = true
    });
    File.WriteAllText(FileName, json);
}

void ListNotes(List<Note> notes)
{
    if (notes.Count == 0)
    {
        Console.WriteLine("Keine Notizen vorhanden.");
        return;
    }

    foreach (var note in notes)
        Console.WriteLine($"[{note.Id}] {note.Title} - {note.CreatedAt:g}");
}

void AddNote(List<Note> notes)
{
    Console.Write("Titel: ");
    string title = Console.ReadLine() ?? "";

    Console.Write("Text: ");
    string text = Console.ReadLine() ?? "";

    int id = notes.Count == 0 ? 1 : notes.Max(n => n.Id) + 1;

    notes.Add(new Note
    {
        Id = id,
        Title = title,
        Text = text,
        CreatedAt = DateTime.Now
    });

    Console.WriteLine("✅ Notiz gespeichert.");
}

void DeleteNote(List<Note> notes)
{
    Console.Write("ID der zu löschenden Notiz: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Ungültige ID.");
        return;
    }

    var note = notes.FirstOrDefault(n => n.Id == id);
    if (note is null)
    {
        Console.WriteLine("Notiz nicht gefunden.");
        return;
    }

    notes.Remove(note);
    Console.WriteLine("🗑️ Notiz gelöscht.");
}

var notesList = LoadNotes();

while (true)
{
    Console.WriteLine("\nJson Notes CLI");
    Console.WriteLine("1) Notizen anzeigen");
    Console.WriteLine("2) Neue Notiz");
    Console.WriteLine("3) Notiz löschen");
    Console.WriteLine("0) Beenden");
    Console.Write("Auswahl: ");

    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            ListNotes(notesList);
            break;
        case "2":
            AddNote(notesList);
            SaveNotes(notesList);
            break;
        case "3":
            DeleteNote(notesList);
            SaveNotes(notesList);
            break;
        case "0":
            SaveNotes(notesList);
            return;
        default:
            Console.WriteLine("Ungültige Auswahl.");
            break;
    }
}

class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
