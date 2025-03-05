//using System;
//using System.Collections.Concurrent;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;

//namespace SmartParkingSystemUI
//{
//	public class CarStatus
//	{
//		public string Status { get; set; }
//		public DateTime StartWaitingTime { get; set; }
//		public TimeSpan WaitingTime { get; set; }
//	}

//	public class SmartParkingSystem
//	{
//		private readonly SemaphoreSlim entryGateSemaphore = new SemaphoreSlim(2); // Max 2 cars entering at once
//		private readonly SemaphoreSlim exitGateSemaphore = new SemaphoreSlim(2);  // Max 2 cars exiting at once
//		private readonly SemaphoreSlim parkingSpotsSemaphore = new SemaphoreSlim(5); // Total parking spots
//		private readonly ConcurrentDictionary<string, CarStatus> carStatuses = new ConcurrentDictionary<string, CarStatus>();

//		private void LogProcessAndThreadInfo(string message)
//		{
//			int processId = Process.GetCurrentProcess().Id;
//			int threadId = Thread.CurrentThread.ManagedThreadId;
//			Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {message} (Process ID: {processId}, Thread ID: {threadId})");
//		}

//		private void UpdateCarStatus(string carNumber, string status)
//		{
//			var now = DateTime.Now;
//			if (!carStatuses.ContainsKey(carNumber))
//			{
//				carStatuses[carNumber] = new CarStatus { Status = status, StartWaitingTime = now };
//			}
//			else
//			{
//				var carStatus = carStatuses[carNumber];
//				carStatus.WaitingTime = now - carStatus.StartWaitingTime;
//				carStatus.Status = status;
//			}

//			// Print current system status
//			Console.WriteLine("\n=== Current System Status ===");
//			foreach (var car in carStatuses)
//			{
//				Console.WriteLine($"Car {car.Key}: {car.Value.Status}, Waiting Time: {car.Value.WaitingTime.TotalSeconds:F2} seconds");
//			}
//			Console.WriteLine("==========================\n");
//		}

//		public async Task CheckTicketAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Checking ticket");
//			await Task.Delay(2000);
//			LogProcessAndThreadInfo($"[Car {carNumber}] Ticket is valid!");
//		}

//		public async Task OpenEntryBarrierAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Waiting for entry gate");
//			await entryGateSemaphore.WaitAsync();
//			try
//			{
//				UpdateCarStatus(carNumber, "Entering through gate");
//				LogProcessAndThreadInfo($"[Car {carNumber}] Opening entry barrier...");
//				await Task.Delay(1500);
//				LogProcessAndThreadInfo($"[Car {carNumber}] Entry barrier opened.");
//			}
//			finally
//			{
//				entryGateSemaphore.Release();
//			}
//		}

//		public async Task ParkCarAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Waiting for parking spot");
//			await parkingSpotsSemaphore.WaitAsync();
//			try
//			{
//				UpdateCarStatus(carNumber, "Parking");
//				LogProcessAndThreadInfo($"[Car {carNumber}] Searching for a parking spot...");
//				await Task.Delay(2000);
//				UpdateCarStatus(carNumber, "Parked");
//			}
//			catch
//			{
//				parkingSpotsSemaphore.Release();
//				throw;
//			}
//		}

//		public async Task ProcessPaymentAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Processing payment");
//			LogProcessAndThreadInfo($"\n[Car {carNumber}] Processing payment...");
//			await Task.Delay(2000);
//			LogProcessAndThreadInfo($"[Car {carNumber}] Payment successful!");
//		}

//		public async Task OpenExitBarrierAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Waiting for exit gate");
//			await exitGateSemaphore.WaitAsync();
//			try
//			{
//				UpdateCarStatus(carNumber, "Exiting through gate");
//				LogProcessAndThreadInfo($"[Car {carNumber}] Opening exit barrier...");
//				await Task.Delay(1500);
//				LogProcessAndThreadInfo($"[Car {carNumber}] Exit barrier opened.");
//			}
//			finally
//			{
//				exitGateSemaphore.Release();
//			}
//		}

//		public async Task UpdateDatabaseAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Updating database");
//			LogProcessAndThreadInfo($"[Car {carNumber}] Updating database...");
//			await Task.Delay(2000);
//			LogProcessAndThreadInfo($"[Car {carNumber}] Database updated.");
//		}

//		public async Task CarEnterAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Waiting to enter");
//			await CheckTicketAsync(carNumber);
//			await OpenEntryBarrierAsync(carNumber);
//			await ParkCarAsync(carNumber);
//			LogProcessAndThreadInfo($"[Car {carNumber}] Successfully parked.");
//		}

//		public async Task CarExitAsync(string carNumber)
//		{
//			UpdateCarStatus(carNumber, "Preparing to exit");
//			await ProcessPaymentAsync(carNumber);
//			await OpenExitBarrierAsync(carNumber);
//			await UpdateDatabaseAsync(carNumber);
//			parkingSpotsSemaphore.Release();
//			UpdateCarStatus(carNumber, "Exited");
//			LogProcessAndThreadInfo($"[Car {carNumber}] Successfully exited.\n");
//		}

//		static async Task Main(string[] args)
//		{
//			SmartParkingSystem parking = new SmartParkingSystem();
//			Console.WriteLine("Smart Parking System is starting...\n");

//			// Create a list of cars that will enter the parking lot
//			var cars = new[] { "A123", "B456", "C789", "D012", "E345", "F678", "G901" };
//			var random = new Random();

//			// Start tasks for all cars trying to enter
//			var tasks = new List<Task>();
//			foreach (var car in cars)
//			{
//				// Random delay before each car attempts to enter
//				await Task.Delay(random.Next(0, 2000));
//				tasks.Add(Task.Run(async () =>
//				{
//					await parking.CarEnterAsync(car);
//					// Random parking duration
//					await Task.Delay(random.Next(3000, 8000));
//					await parking.CarExitAsync(car);
//				}));
//			}

//			await Task.WhenAll(tasks);
//			Console.WriteLine("Parking system transaction completed!\n");
//		}
//	}
//}

//// bãi giữ xe có 2 cổng vào 2 cổng ra random các xe có sẵn và các xe chuẩn bị vào tính thời gian xe sẵn sàng đi vào đến lúc vào được là bao nhiêu
///
