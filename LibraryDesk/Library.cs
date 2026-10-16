using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LibraryDesk
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Borrower { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Issued { get; set; }
    }

    // A simple, readable local data format. No database setup required.
    public class LibraryData
    {
        public LibraryData()
        {
            Books = new List<Book>();
        }

        public List<Book> Books { get; set; }
    }

    public sealed class Library
    {
        private readonly List<Book> books;
        private readonly string filePath;
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(LibraryData));

        public string DataPath { get { return filePath; } }

        public Library(string path = null)
        {
            filePath = path ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "LibraryDesk", "library.xml");

            if (File.Exists(filePath))
            {
                // Intentionally fail on corrupt files rather than silently overwriting user data.
                using (var stream = File.OpenRead(filePath))
                {
                    var data = (LibraryData)Serializer.Deserialize(stream);
                    if (data == null || data.Books == null)
                        throw new InvalidDataException("Library data is missing its book list.");
                    books = data.Books;
                }
                if (books.Any(b => b == null || b.Id <= 0 || b.Title == null || b.Author == null) ||
                    books.GroupBy(b => b.Id).Any(g => g.Count() > 1))
                    throw new InvalidDataException("Library data contains invalid or duplicate records.");
            }
            else
            {
                books = new List<Book>
                {
                    new Book { Id = 1, Title = "Atomic Habits", Author = "James Clear" },
                    new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt & David Thomas" },
                    new Book { Id = 3, Title = "Clean Code", Author = "Robert C. Martin" },
                    new Book { Id = 4, Title = "Deep Work", Author = "Cal Newport" },
                    new Book { Id = 5, Title = "The Design of Everyday Things", Author = "Don Norman" },
                    new Book { Id = 6, Title = "The Alchemist", Author = "Paulo Coelho",
                        Issued = true, Borrower = "Demo Reader", DueDate = DateTime.Today.AddDays(8) }
                };
                Save();
            }
        }

        public List<Book> GetBooks()
        {
            // Return copies to prevent edits that bypass Save().
            return books.Select(Copy).OrderBy(b => b.Id).ToList();
        }

        public void AddBook(string title, string author)
        {
            title = (title ?? "").Trim();
            author = (author ?? "").Trim();
            if (title.Length == 0 || author.Length == 0)
                throw new ArgumentException("Enter both the book title and author.");
            if (title.Length > 120 || author.Length > 100)
                throw new ArgumentException("Please shorten the book title or author name.");

            var nextId = books.Count == 0 ? 1 : checked(books.Max(b => b.Id) + 1);
            var book = new Book { Id = nextId, Title = title, Author = author };
            books.Add(book);
            try { Save(); }
            catch { books.Remove(book); throw; }
        }

        public void DeleteBook(int id)
        {
            var book = Find(id);
            if (book.Issued)
                throw new InvalidOperationException("Return this book before removing it.");

            int index = books.IndexOf(book);
            books.RemoveAt(index);
            try { Save(); }
            catch { books.Insert(index, book); throw; }
        }

        public void IssueBook(int id, string borrower)
        {
            borrower = (borrower ?? "").Trim();
            if (borrower.Length == 0 || borrower.Length > 100)
                throw new ArgumentException("Enter a borrower name (up to 100 characters).");

            var book = Find(id);
            if (book.Issued)
                throw new InvalidOperationException("This book is already issued.");

            book.Issued = true;
            book.Borrower = borrower;
            book.DueDate = DateTime.Today.AddDays(14);
            try { Save(); }
            catch { book.Issued = false; book.Borrower = null; book.DueDate = null; throw; }
        }

        public void ReturnBook(int id)
        {
            var book = Find(id);
            if (!book.Issued)
                throw new InvalidOperationException("This book is already available.");

            var borrower = book.Borrower;
            var dueDate = book.DueDate;
            book.Issued = false;
            book.Borrower = null;
            book.DueDate = null;
            try { Save(); }
            catch { book.Issued = true; book.Borrower = borrower; book.DueDate = dueDate; throw; }
        }

        private Book Find(int id)
        {
            var book = books.Find(b => b.Id == id);
            if (book == null)
                throw new InvalidOperationException("Select a valid book first.");
            return book;
        }

        private void Save()
        {
            var folder = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(folder))
                throw new InvalidOperationException("A storage folder is required.");
            Directory.CreateDirectory(folder);

            // Write a complete temporary file before replacing the current data file.
            var temp = filePath + ".tmp";
            using (var stream = new FileStream(temp, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Serializer.Serialize(stream, new LibraryData { Books = books });
                stream.Flush(true);
            }
            if (File.Exists(filePath))
                File.Replace(temp, filePath, filePath + ".bak");
            else
                File.Move(temp, filePath);
        }

        private static Book Copy(Book b)
        {
            return new Book { Id = b.Id, Title = b.Title, Author = b.Author,
                Issued = b.Issued, Borrower = b.Borrower, DueDate = b.DueDate };
        }
    }
}
