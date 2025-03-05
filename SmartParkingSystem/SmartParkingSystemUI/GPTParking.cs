using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SmartParkingSystemUI
{
	public class ParkingConfig
	{
		public int MaxEntry { get; set; }
		public int MaxExit { get; set; }
		public int TotalSpots { get; set; }
		public int TicketCheckDelay { get; set; }
		public int EntryDelay { get; set; }
		public int ParkingDelay { get; set; }
		public int PaymentDelay { get; set; }
		public int ExitDelay { get; set; }
		public int DatabaseUpdateDelay { get; set; }
	}

	public class CarStatus
	{
		public string Status { get; set; }
		public DateTime StartWaitingTime { get; set; }
		public TimeSpan WaitingTime { get; set; }
	}

	public interface IBarrierService
	{
		Task OpenEntryBarrierAsync(string carNumber, CancellationToken cancellationToken);
		Task OpenExitBarrierAsync(string carNumber, CancellationToken cancellationToken);
	}

	public class BarrierService : IBarrierService
	{
		private readonly SemaphoreSlim _entrySemaphore;
		private readonly SemaphoreSlim _exitSemaphore;
		private readonly ParkingConfig _config;

		public BarrierService(ParkingConfig config)
		{
			_config = config;
			_entrySemaphore = new SemaphoreSlim(config.MaxEntry);
			_exitSemaphore = new SemaphoreSlim(config.MaxExit);
		}

		public async Task OpenEntryBarrierAsync(string carNumber, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[Car {carNumber}] Waiting for entry gate.");
			await _entrySemaphore.WaitAsync(cancellationToken);
			try
			{
				Console.WriteLine($"[Car {carNumber}] Entering through gate...");
				await Task.Delay(_config.EntryDelay, cancellationToken);
				Console.WriteLine($"[Car {carNumber}] Entry barrier opened.");
			}
			finally
			{
				_entrySemaphore.Release();
			}
		}

		public async Task OpenExitBarrierAsync(string carNumber, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[Car {carNumber}] Waiting for exit gate.");
			await _exitSemaphore.WaitAsync(cancellationToken);
			try
			{
				Console.WriteLine($"[Car {carNumber}] Exiting through gate...");
				await Task.Delay(_config.ExitDelay, cancellationToken);
				Console.WriteLine($"[Car {carNumber}] Exit barrier opened.");
			}
			finally
			{
				_exitSemaphore.Release();
			}
		}
	}

	public interface ICarProcessService
	{
		Task CheckTicketAsync(string carNumber, CancellationToken cancellationToken);
		Task ParkCarAsync(string carNumber, CancellationToken cancellationToken);
		Task ProcessPaymentAsync(string carNumber, CancellationToken cancellationToken);
		Task UpdateDatabaseAsync(string carNumber, CancellationToken cancellationToken);
	}

	public class CarProcessService : ICarProcessService
	{
		private readonly ParkingConfig _config;

		public CarProcessService(ParkingConfig config)
		{
			_config = config;
		}

		public async Task CheckTicketAsync(string carNumber, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[Car {carNumber}] Checking ticket...");
			await Task.Delay(_config.TicketCheckDelay, cancellationToken);
			Console.WriteLine($"[Car {carNumber}] Ticket is valid!");
		}

		public async Task ParkCarAsync(string carNumber, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[Car {carNumber}] Searching for a parking spot...");
			await Task.Delay(_config.ParkingDelay, cancellationToken);
			Console.WriteLine($"[Car {carNumber}] Parked.");
		}

		public async Task ProcessPaymentAsync(string carNumber, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[Car {carNumber}] Processing payment...");
			await Task.Delay(_config.PaymentDelay, cancellationToken);
			Console.WriteLine($"[Car {carNumber}] Payment successful!");
		}

		public async Task UpdateDatabaseAsync(string carNumber, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[Car {carNumber}] Updating database...");
			await Task.Delay(_config.DatabaseUpdateDelay, cancellationToken);
			Console.WriteLine($"[Car {carNumber}] Database updated.");
		}
	}

	public class ParkingLotManager
	{
		private readonly IBarrierService _barrierService;
		private readonly ICarProcessService _processService;
		private readonly SemaphoreSlim _parkingSpotsSemaphore;
		private readonly ConcurrentDictionary<string, CarStatus> _carStatuses;

		public ParkingLotManager(ParkingConfig config, IBarrierService barrierService, ICarProcessService processService)
		{
			_barrierService = barrierService;
			_processService = processService;
			_parkingSpotsSemaphore = new SemaphoreSlim(config.TotalSpots);
			_carStatuses = new ConcurrentDictionary<string, CarStatus>();
		}

		private void UpdateCarStatus(string carNumber, string status)
		{
			var now = DateTime.Now;
			_carStatuses.AddOrUpdate(carNumber,
				new CarStatus { Status = status, StartWaitingTime = now },
				(_, existing) =>
				{
					existing.WaitingTime = now - existing.StartWaitingTime;
					existing.Status = status;
					return existing;
				});

			Console.WriteLine("\n=== Current System Status ===");
			foreach (var car in _carStatuses)
			{
				Console.WriteLine($"Car {car.Key}: {car.Value.Status}, Waiting Time: {car.Value.WaitingTime.TotalSeconds:F2} seconds");
			}
			Console.WriteLine("===============================\n");
		}

		public async Task CarEnterAsync(string carNumber, CancellationToken cancellationToken)
		{
			UpdateCarStatus(carNumber, "Waiting to enter");
			await _processService.CheckTicketAsync(carNumber, cancellationToken);
			await _barrierService.OpenEntryBarrierAsync(carNumber, cancellationToken);
			await _parkingSpotsSemaphore.WaitAsync(cancellationToken);
			UpdateCarStatus(carNumber, "Parking");
			await _processService.ParkCarAsync(carNumber, cancellationToken);
			UpdateCarStatus(carNumber, "Parked");
			Console.WriteLine($"[Car {carNumber}] Successfully parked.");
		}

		public async Task CarExitAsync(string carNumber, CancellationToken cancellationToken)
		{
			UpdateCarStatus(carNumber, "Preparing to exit");
			await _processService.ProcessPaymentAsync(carNumber, cancellationToken);
			await _barrierService.OpenExitBarrierAsync(carNumber, cancellationToken);
			await _processService.UpdateDatabaseAsync(carNumber, cancellationToken);
			_parkingSpotsSemaphore.Release();
			UpdateCarStatus(carNumber, "Exited");
			Console.WriteLine($"[Car {carNumber}] Successfully exited.\n");
		}
	}

	public class Program
	{
		public static async Task Main(string[] args)
		{
			var parkingConfig = new ParkingConfig
			{
				MaxEntry = 2,
				MaxExit = 2,
				TotalSpots = 5,
				TicketCheckDelay = 2000,
				EntryDelay = 1500,
				ParkingDelay = 2000,
				PaymentDelay = 2000,
				ExitDelay = 1500,
				DatabaseUpdateDelay = 2000
			};

			// Khởi tạo các dịch vụ thủ công (không dùng logger)
			IBarrierService barrierService = new BarrierService(parkingConfig);
			ICarProcessService processService = new CarProcessService(parkingConfig);
			ParkingLotManager parkingManager = new ParkingLotManager(parkingConfig, barrierService, processService);

			var cancellationTokenSource = new CancellationTokenSource();
			var random = new Random();
			var cars = new[] { "A123", "B456", "C789", "D012", "E345", "F678", "G901" };
			var tasks = new ConcurrentBag<Task>();

			foreach (var car in cars)
			{
				// Delay ngẫu nhiên cho từng xe
				await Task.Delay(random.Next(0, 2000));
				var task = Task.Run(async () =>
				{
					try
					{
						await parkingManager.CarEnterAsync(car, cancellationTokenSource.Token);
						// Delay mô phỏng thời gian xe đỗ
						await Task.Delay(random.Next(3000, 8000), cancellationTokenSource.Token);
						await parkingManager.CarExitAsync(car, cancellationTokenSource.Token);
					}
					catch (OperationCanceledException)
					{
						Console.WriteLine($"Operation for car {car} was cancelled.");
					}
				}, cancellationTokenSource.Token);
				tasks.Add(task);
			}

			await Task.WhenAll(tasks);
			Console.WriteLine("Parking system simulation completed!");
		}
	}
}
