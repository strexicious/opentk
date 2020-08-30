﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK.Compute.OpenCL;

namespace OpenToolkit.OpenCL.Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			//Get the ids of available opencl platforms
			CL.GetPlatformIds(out IntPtr[] platformIds);

			//Get the device ids for each platform
			foreach (IntPtr platformId in platformIds)
			{
				CL.GetDeviceIds(platformId, DeviceType.All, out IntPtr[] deviceIds);

				IntPtr context = CL.CreateContext(IntPtr.Zero, (uint)deviceIds.Length, deviceIds, IntPtr.Zero,
					IntPtr.Zero, out CLResultCode result);
				if (result != CLResultCode.Success)
				{
					throw new Exception("The context couldn't be created.");
				}

				string code = @"
                __kernel void add(__global float* A, __global float* B,__global float* result)
                {
                    int i = get_global_id(0);
                    result[i] = A[i] + B[i];
                }";

				IntPtr program = CL.CreateProgramWithSource(context, code, out result);
				CL.BuildProgram(program, (uint)deviceIds.Length, deviceIds, null, IntPtr.Zero, IntPtr.Zero);

				IntPtr kernel = CL.CreateKernel(program, "add", out result);

				int arraySize = 2000;
				float[] A = new float[arraySize];
				float[] B = new float[arraySize];

				for (int i = 0; i < arraySize; i++)
				{
					A[i] = i + 1;
					B[i] = i;
				}

				IntPtr bufferA = CL.CreateBuffer(context, MemoryFlags.ReadOnly | MemoryFlags.CopyHostPtr, A,
					out result);
				IntPtr bufferB = CL.CreateBuffer(context, MemoryFlags.ReadOnly | MemoryFlags.CopyHostPtr, B,
					out result);
				IntPtr resultBuffer = CL.CreateBuffer(context, MemoryFlags.WriteOnly,
					new UIntPtr((uint)(arraySize * sizeof(float))), IntPtr.Zero, out result);
				UIntPtr bufferSize = (UIntPtr)UIntPtr.Size;

				try
				{
					GCHandle inputA = GCHandle.Alloc(bufferA, GCHandleType.Pinned);
					GCHandle inputB = GCHandle.Alloc(bufferB, GCHandleType.Pinned);
					GCHandle resultGC = GCHandle.Alloc(resultBuffer, GCHandleType.Pinned);
					CL.SetKernelArg(kernel, 0, bufferSize, inputA.AddrOfPinnedObject());
					CL.SetKernelArg(kernel, 1, bufferSize, inputB.AddrOfPinnedObject());
					CL.SetKernelArg(kernel, 2, bufferSize, resultGC.AddrOfPinnedObject());

					// IntPtr commandQueue = CL.CreateCommandQueue(context, deviceIds[0], 0, out result);
					IntPtr commandQueue =
						CL.CreateCommandQueueWithProperties(context, deviceIds[0], IntPtr.Zero, out result);

					CL.EnqueueNDRangeKernel(commandQueue, kernel, 1, null, new IntPtr[] {new IntPtr(A.Length)},
						null, 0, null,  out IntPtr eventHandle);

					CL.Finish(commandQueue);

					CL.SetEventCallback(eventHandle, (int)CommandExecutionStatus.Complete, (waitEvent, data) =>
					{
						CL.EnqueueReadBuffer(commandQueue, resultBuffer, false, UIntPtr.Zero, arraySize,
							out float[] resultValues, 0, null, out _);

						StringBuilder line = new StringBuilder();
						foreach (float res in resultValues)
						{
							line.Append(res);
							line.Append(", ");
						}

						Console.WriteLine(line.ToString());
					});

					//get rid of the buffers because we no longer need them
					CL.ReleaseMemObject(bufferA);
					CL.ReleaseMemObject(bufferB);
					CL.ReleaseMemObject(resultBuffer);

					//Release the program kernels and queues
					CL.ReleaseProgram(program);
					CL.ReleaseKernel(kernel);
					CL.ReleaseCommandQueue(commandQueue);
					CL.ReleaseContext(context);
					CL.ReleaseEvent(eventHandle);


					//Get rid of the GC handles too
					inputA.Free();
					inputB.Free();
					resultGC.Free();

				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					throw;
				}

			}
		}
	}
}
