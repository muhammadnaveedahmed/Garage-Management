using System.Collections;

namespace Garage.UIConsole.Entities;

public class Garage<T> : IGarage<T>
    where T : IVehicle
{
    private T[] vehicles;
    private uint Capacity { get; init; }
    public bool IsFull => Capacity <= NumberOfVehicles;
    public uint NumberOfVehicles { get; private set; }
    public T this[int index] => vehicles[index];
    public Garage(uint capacity)
    {
        if (capacity == 0) throw new ArgumentException();

        Capacity = capacity;

        vehicles = new T[Capacity];
    }

    public bool Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (IsFull) return false;
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] is null)
            {
                vehicles[i] = item;
                NumberOfVehicles++;
                return true;
            }
        }

        return false;

    }

    public bool Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var vehicleIndex = Array.IndexOf(vehicles, item);
        if (vehicleIndex == -1) return false;

        Array.Clear(vehicles, vehicleIndex, 1);
        NumberOfVehicles--;
        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var vehicle in vehicles)
            yield return vehicle;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}