// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Car car = new Car();
car.PlateNumber = "53X9-12345";

Car.Wheel wheel = new Car.Wheel(car);
wheel.Print();

car.Run(3);//Mode 3- velocity = 60km/h
car.Run(55.0);//Run with velocity = 55km/h




// 4 characteristics: Encapsulation, Inheritance, Polymorphism, Abstraction

//Encapsulation: 

//System==> Sofware ===> Package ===> Class ==> Attribute + Method
int a = 3;
Car electricCar = new ElectricCar("SN123456","53X9-112345",60,20000);
int b = 3;

car.StartEngine();//Xe xang 
electricCar.StartEngine();//xe dien

Ferry ferry = new Ferry();
ferry.Take(new Bike());
ferry.Take(new Bike());
ferry.Take(new Car());
ferry.Take(new ElectricCar("a","a",4,20000));

ferry.PassRiver();


class Ferry
{
    List<IVehicle> vehicles = new List<IVehicle>();
    public void Take(IVehicle vehicle)
    { 
        vehicles.Add(vehicle);
    }

    public void PassRiver()
    {
        foreach (var item in vehicles)
        {
            item.Run();
        }
    }
}


interface IVehicle
{
    void Run();
}


class Bike : IVehicle
{
    public string PlateNumber { get; set; }

    void IVehicle.Run()
    {
        Console.WriteLine("Bike run!");
    }
}

class Car: IVehicle
{
    private string serialNumber;
    protected string plateNumber;//Field: trường dữ liệu- heap

    public Car()
    {
        
    }

    public Car(string serialNum, string plateNum, int wheelSize)
    {
       this.serialNumber = serialNum;
        this.plateNumber = plateNum;
        this.Wheels = new List<Wheel>();
        for (int i = 0; i < 4; i++)
        {
            Wheel wheel1 = new Wheel(this);
            wheel1.WheelSize = wheelSize;
            this.Wheels.Add(wheel1);
        }
       
       ;
    }

    public string PlateNumber
    {
        get { return plateNumber; }
        set
        {
            plateNumber = value;
        }
    }

    //Demo Overload

    /// <summary>
    /// Run car with mode 
    /// </summary>
    /// <param name="mode">Value from 0 to 5</param>
    public void Run(int mode)
    {
        Console.WriteLine("Car is running in mode {0} with velocity {1}km/h", mode, mode * 20);
    }

    public void Run(double velocity)
    {
        Console.WriteLine("Car is running in  with velocity {0}km/h", velocity);
    }

    public virtual void StartEngine()
    {
        Console.WriteLine("Turn on!");
        Console.WriteLine("Engine Warmup!");
        Console.WriteLine("Engine Ready1");

    }

    public void Run()
    {
        Console.WriteLine("Car Run!");
    }

    public List<Wheel> Wheels { get; set; }//Property: thuộc tính

    internal class Wheel
    {
        private Car _owner;   
        public int WheelSize { get; set; }

        public Wheel(Car owner)
        {
            _owner = owner;
        }

        public void Print()
        {
            string pn; //variable: stack
            Console.WriteLine("WheelSize: {0}cm of car with platnumber {1}",
                WheelSize,_owner.plateNumber);
        }

        
    }
}

class ElectricCar :Car
{
    public int PinCapacity { get; set; }

    public ElectricCar(string serialNum, string plateNum, int wheelSize,int pinCapacity)
        :base(serialNum,plateNum,wheelSize)
    {
        this.PinCapacity = pinCapacity;
    }

    public override void StartEngine()
    {
        Console.WriteLine("Turn on and engine ready!!!");

    }

}
