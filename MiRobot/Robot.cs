namespace MiRobot
{
    public class Robot
    {
        public int Id;
        public string Name;
        public string Status;
        public Position Position = new Position(0, 0, 0);

        public Robot(int id, string name)
        {
            Id = id;
            Name = name;
            Status = "Online";
        }
    }

    public record Position(double X, double Y, double Z)
    {
        public bool IsValid() =>
            double.IsFinite(X) &&
            double.IsFinite(Y) &&
            double.IsFinite(Z) &&
            Math.Abs(X) <= 1000 &&
            Math.Abs(Y) <= 1000 &&
            Math.Abs(Z) <= 1000;
    }
}
