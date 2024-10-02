namespace Task_Manager_HttpListener_NP;
public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public override string ToString()
    {
        return $"{Id} {Model}";
    }
}
