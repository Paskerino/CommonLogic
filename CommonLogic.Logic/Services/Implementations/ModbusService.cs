using CommonLogic.Core.Models;
using CommonLogic.Services.Interfaces;
using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Services.Implementations
{
    public class ModbusService : IModbusService
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public async Task<List<SensorReading>> ReadRegistersAsync(Device device)
        {
            if (device.Sensors == null || !device.Sensors.Any())
                return new List<SensorReading>();

            try
            {
                // Визначаємо, який блок регістрів нам потрібно прочитати
                ushort startAddress = device.Sensors.Min(s => s.RegisterAddress);
                ushort endAddress = device.Sensors.Max(s => s.RegisterAddress);
                ushort numRegisters = (ushort)(endAddress - startAddress + 1);

                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(device.IpAddress, device.Port);

                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateMaster(client);

                    // Робимо ОДИН запит на весь блок
                    ushort[] rawData = await master.ReadHoldingRegistersAsync(device.SlaveId, startAddress, numRegisters);
                    var readings = new List<SensorReading>();
                    var now = DateTime.Now;

                    // Тепер "розбираємо" отримані дані по наших датчиках
                    foreach (var sensor in device.Sensors)
                    {
                        int index = sensor.RegisterAddress - startAddress;
                        if (index < rawData.Length)
                        {
                            readings.Add(new SensorReading
                            {
                                SensorId = sensor.Id,

                                Value = rawData[index],
                                Timestamp = now
                            });
                        }
                    }
                    return readings;
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ModbusService Error] {ex.Message}");
                return null;
            }
        }

        public async Task<bool> WriteRegisterAsync(Device device , ushort registerAddress, ushort value)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(device.IpAddress, device.Port);

                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateMaster(client);

                    await master.WriteSingleRegisterAsync(device.SlaveId, registerAddress, value);
                }
                   
                return true; // Успіх
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Помилка запису в регістр {registerAddress} пристрою {device.Name}.");
                return false; // Помилка
            }
        }
    }
}
