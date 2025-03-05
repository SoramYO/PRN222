using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelSortingAssignment
{
	class Program
	{
		static void Main(string[] args)
		{
			// Number of iterations and size of each list
			const int iterations = 1000;
			const int listSize = 10000;

			// Counters for the number of times each algorithm was fastest
			int fastestCountQuickSort = 0;
			int fastestCountMergeSort = 0;
			int fastestCountHeapSort = 0;
			int fastestCountBubbleSort = 0;
			int fastestCountBuiltinSort = 0;

			// Cumulative times (in milliseconds) for each algorithm
			double totalTimeQuickSort = 0;
			double totalTimeMergeSort = 0;
			double totalTimeHeapSort = 0;
			double totalTimeBubbleSort = 0;
			double totalTimeBuiltinSort = 0;

			// Create a Random object (used in the main thread only)
			Random rand = new Random();

			// Repeat the process 1,000 times
			for (int iter = 0; iter < iterations; iter++)
			{
				// --- Step 1: Generate random data ---
				List<int> originalList = new List<int>(listSize);
				for (int i = 0; i < listSize; i++)
				{
					originalList.Add(rand.Next(0, 10000));
				}

				// --- Step 2: Clone Data into 5 separate lists ---
				List<int> listQuick = new List<int>(originalList);
				List<int> listMerge = new List<int>(originalList);
				List<int> listHeap = new List<int>(originalList);
				List<int> listBubble = new List<int>(originalList);
				List<int> listBuiltin = new List<int>(originalList);

				// --- Step 3: Create parallel tasks for each sorting algorithm ---
				Task<double> taskQuick = Task.Run(() =>
				{
					Stopwatch sw = Stopwatch.StartNew();
					QuickSort(listQuick, 0, listQuick.Count - 1);
					sw.Stop();
					return sw.Elapsed.TotalMilliseconds;
				});

				Task<double> taskMerge = Task.Run(() =>
				{
					Stopwatch sw = Stopwatch.StartNew();
					MergeSort(listMerge);
					sw.Stop();
					return sw.Elapsed.TotalMilliseconds;
				});

				Task<double> taskHeap = Task.Run(() =>
				{
					Stopwatch sw = Stopwatch.StartNew();
					HeapSort(listHeap);
					sw.Stop();
					return sw.Elapsed.TotalMilliseconds;
				});

				Task<double> taskBubble = Task.Run(() =>
				{
					Stopwatch sw = Stopwatch.StartNew();
					BubbleSort(listBubble);
					sw.Stop();
					return sw.Elapsed.TotalMilliseconds;
				});

				Task<double> taskBuiltin = Task.Run(() =>
				{
					Stopwatch sw = Stopwatch.StartNew();
					listBuiltin.Sort(); // Built-in C# sort
					sw.Stop();
					return sw.Elapsed.TotalMilliseconds;
				});

				// Wait for all tasks to complete
				Task.WaitAll(taskQuick, taskMerge, taskHeap, taskBubble, taskBuiltin);

				// --- Step 4: Retrieve the execution times ---
				double timeQuick = taskQuick.Result;
				double timeMerge = taskMerge.Result;
				double timeHeap = taskHeap.Result;
				double timeBubble = taskBubble.Result;
				double timeBuiltin = taskBuiltin.Result;

				// Update cumulative execution times
				totalTimeQuickSort += timeQuick;
				totalTimeMergeSort += timeMerge;
				totalTimeHeapSort += timeHeap;
				totalTimeBubbleSort += timeBubble;
				totalTimeBuiltinSort += timeBuiltin;

				// Determine which algorithm was the fastest in this iteration.
				// (In case of a tie, the first matching algorithm is counted.)
				double minTime = Math.Min(
					Math.Min(Math.Min(Math.Min(timeQuick, timeMerge), timeHeap), timeBubble),
					timeBuiltin);

				if (timeQuick == minTime)
					fastestCountQuickSort++;
				else if (timeMerge == minTime)
					fastestCountMergeSort++;
				else if (timeHeap == minTime)
					fastestCountHeapSort++;
				else if (timeBubble == minTime)
					fastestCountBubbleSort++;
				else if (timeBuiltin == minTime)
					fastestCountBuiltinSort++;

				// (Optional) Uncomment the line below to print each iteration’s results.
				// Console.WriteLine($"Iteration {iter + 1}: QuickSort={timeQuick:F2}ms, MergeSort={timeMerge:F2}ms, HeapSort={timeHeap:F2}ms, BubbleSort={timeBubble:F2}ms, BuiltinSort={timeBuiltin:F2}ms");
			}

			// --- Final Statistical Analysis ---
			double avgQuickSort = totalTimeQuickSort / iterations;
			double avgMergeSort = totalTimeMergeSort / iterations;
			double avgHeapSort = totalTimeHeapSort / iterations;
			double avgBubbleSort = totalTimeBubbleSort / iterations;
			double avgBuiltinSort = totalTimeBuiltinSort / iterations;

			Console.WriteLine("\nFinal Statistics after " + iterations + " iterations:");
			Console.WriteLine("-----------------------------------------------------");
			Console.WriteLine("Algorithm       Fastest Count   Average Time (ms)");
			Console.WriteLine("-----------------------------------------------------");
			Console.WriteLine($"QuickSort       {fastestCountQuickSort,15}   {avgQuickSort,20:F4}");
			Console.WriteLine($"MergeSort       {fastestCountMergeSort,15}   {avgMergeSort,20:F4}");
			Console.WriteLine($"HeapSort        {fastestCountHeapSort,15}   {avgHeapSort,20:F4}");
			Console.WriteLine($"BubbleSort      {fastestCountBubbleSort,15}   {avgBubbleSort,20:F4}");
			Console.WriteLine($"Built-in Sort   {fastestCountBuiltinSort,15}   {avgBuiltinSort,20:F4}");

			// Pause the console (if needed)
			Console.WriteLine("\nPress any key to exit.");
			Console.ReadKey();
		}

		#region Sorting Algorithm Implementations

		/// <summary>
		/// Implements QuickSort using recursion.
		/// </summary>
		static void QuickSort(List<int> arr, int low, int high)
		{
			if (low < high)
			{
				int pivotIndex = Partition(arr, low, high);
				QuickSort(arr, low, pivotIndex - 1);
				QuickSort(arr, pivotIndex + 1, high);
			}
		}

		static int Partition(List<int> arr, int low, int high)
		{
			int pivot = arr[high];
			int i = low - 1;
			for (int j = low; j < high; j++)
			{
				if (arr[j] < pivot)
				{
					i++;
					Swap(arr, i, j);
				}
			}
			Swap(arr, i + 1, high);
			return i + 1;
		}

		/// <summary>
		/// Implements MergeSort.
		/// </summary>
		static void MergeSort(List<int> arr)
		{
			if (arr.Count <= 1)
				return;
			MergeSortHelper(arr, 0, arr.Count - 1);
		}

		static void MergeSortHelper(List<int> arr, int left, int right)
		{
			if (left < right)
			{
				int mid = left + (right - left) / 2;
				MergeSortHelper(arr, left, mid);
				MergeSortHelper(arr, mid + 1, right);
				Merge(arr, left, mid, right);
			}
		}

		static void Merge(List<int> arr, int left, int mid, int right)
		{
			int n1 = mid - left + 1;
			int n2 = right - mid;
			int[] leftArray = new int[n1];
			int[] rightArray = new int[n2];

			for (int i = 0; i < n1; i++)
				leftArray[i] = arr[left + i];
			for (int j = 0; j < n2; j++)
				rightArray[j] = arr[mid + 1 + j];

			int iIndex = 0, jIndex = 0, k = left;
			while (iIndex < n1 && jIndex < n2)
			{
				if (leftArray[iIndex] <= rightArray[jIndex])
				{
					arr[k] = leftArray[iIndex];
					iIndex++;
				}
				else
				{
					arr[k] = rightArray[jIndex];
					jIndex++;
				}
				k++;
			}
			while (iIndex < n1)
			{
				arr[k] = leftArray[iIndex];
				iIndex++;
				k++;
			}
			while (jIndex < n2)
			{
				arr[k] = rightArray[jIndex];
				jIndex++;
				k++;
			}
		}

		/// <summary>
		/// Implements HeapSort.
		/// </summary>
		static void HeapSort(List<int> arr)
		{
			int n = arr.Count;

			// Build heap (rearrange array)
			for (int i = n / 2 - 1; i >= 0; i--)
				Heapify(arr, n, i);

			// One by one extract an element from the heap
			for (int i = n - 1; i >= 0; i--)
			{
				Swap(arr, 0, i);
				Heapify(arr, i, 0);
			}
		}

		static void Heapify(List<int> arr, int n, int i)
		{
			int largest = i;
			int left = 2 * i + 1;
			int right = 2 * i + 2;

			if (left < n && arr[left] > arr[largest])
				largest = left;
			if (right < n && arr[right] > arr[largest])
				largest = right;
			if (largest != i)
			{
				Swap(arr, i, largest);
				Heapify(arr, n, largest);
			}
		}

		/// <summary>
		/// Implements BubbleSort.
		/// </summary>
		static void BubbleSort(List<int> arr)
		{
			int n = arr.Count;
			for (int i = 0; i < n - 1; i++)
			{
				for (int j = 0; j < n - i - 1; j++)
				{
					if (arr[j] > arr[j + 1])
						Swap(arr, j, j + 1);
				}
			}
		}

		/// <summary>
		/// Utility method to swap two elements in a list.
		/// </summary>
		static void Swap(List<int> arr, int i, int j)
		{
			int temp = arr[i];
			arr[i] = arr[j];
			arr[j] = temp;
		}

		#endregion
	}
}
