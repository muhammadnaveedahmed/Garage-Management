namespace Garage.UIConsole.Entities;

public interface IGarage<T> : IEnumerable<T>
    where T : IVehicle
{
    T this[int index] { get; }

    uint NumberOfVehicles { get; }
    bool IsFull { get; }

    bool Add(T item);
    bool Remove(T item);
}
