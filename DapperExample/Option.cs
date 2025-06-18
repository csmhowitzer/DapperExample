namespace DapperExample;

public class Option<T> : IEquatable<Option<T>> where T : class
{
    private T? _content;

    private Option() { }

    public static Option<T> Some(T obj) => new() { _content = obj };
    public static Option<T> None() => new();

    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
        new() { _content = _content is not null ? map(_content) : null };

    public Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) where TResult : class =>
        _content is not null ? map(_content) : Option<TResult>.None();

    public T Reduce(T orElse) => _content ?? orElse;
    public T Reduce(Func<T> orElse) => _content ?? orElse();

    public Option<T> Where(Func<T, bool> predicate) => _content is not null && predicate(_content) ? this : Option<T>.None();
    public Option<T> WhereNot(Func<T, bool> predicate) => _content is not null && !predicate(_content) ? this : Option<T>.None();

    public override int GetHashCode() => _content?.GetHashCode() ?? 0;
    public override bool Equals(object? other) => Equals(other as Option<T>);

    public bool Equals(Option<T>? other) =>
        other is not null && (_content?.Equals(other._content) ?? false);

    public static bool operator ==(Option<T>? a, Option<T>? b) => a is null ? b is null : a.Equals(b);
    public static bool operator !=(Option<T>? a, Option<T>? b) => !(a == b);
}
// Extension options
public static class OptionalExtensions
{
    // convert any T to Option<T>
    public static Option<T> ToOption<T>(this T? obj) where T : class => obj is not null ? Option<T>.Some(obj) : Option<T>.None();
    // Treat any T like an optional and apply a predicate like LINQ
    public static Option<T> Where<T>(this T? obj, Func<T, bool> predicate) where T : class =>
        obj is not null && predicate(obj) ? Option<T>.Some(obj) : Option<T>.None();
    public static Option<T> WhereNot<T>(this T? obj, Func<T, bool> predicate) where T : class =>
        obj is not null && !predicate(obj) ? Option<T>.Some(obj) : Option<T>.None();
}

class Person
{
    public string FirstName { get; set; }
    public Option<string> LastName { get; set; }

    private Person(string firstName, Option<string> lastName) =>
        (FirstName, LastName) = (firstName, lastName);

    public static Person Create(string firstName, string lastName) => new(firstName, Option<string>.Some(lastName));
    public static Person Create(string name) => new(name, Option<string>.None());

    public static string GetLabel(Person person) => person
        .LastName
        .Map(lastName => $"{person.FirstName} {lastName}")
        .Reduce(person.FirstName);
}

class Book
{
    public string Title { get; set; }
    public Option<Person> Author { get; set; }

    private Book(string title, Option<Person> author) => (Title, Author) = (title, author);

    public static Book Create(string title, Person author) => new(title, Option<Person>.Some(author));
    public static Book Create(string title) => new(title, Option<Person>.None());

    public static string GetLabel(Person person) => person
        .LastName
        .Map(lastName => $"{person.FirstName} {lastName}")
        .Reduce(person.FirstName);

    public static string GetBookLabel(Book book) => book
        .Author
        .Map(GetLabel)
        .Map(author => $"{book.Title} by {author}")
        .Reduce(book.Title);
}
