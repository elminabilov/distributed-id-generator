namespace Snowflake.SnowflakeAlgorithm;

public sealed class SnowflakeIdGenerator
{
    // Epoch: 2010-11-04 01:42:54 UTC
    private const long Epoch = 1288834974657L;

    private const int DatacenterIdBits = 5;
    private const int MachineIdBits = 5;
    private const int SequenceBits = 12;

    private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits); // 31
    private const long MaxMachineId = -1L ^ (-1L << MachineIdBits);       // 31
    private const long MaxSequence = -1L ^ (-1L << SequenceBits);         // 4095

    private const int MachineIdShift = SequenceBits;
    private const int DatacenterIdShift = SequenceBits + MachineIdBits;
    private const int TimestampShift = SequenceBits + MachineIdBits + DatacenterIdBits;

    private readonly long _datacenterId;
    private readonly long _machineId;

    private long _lastTimestamp = -1L;
    private long _sequence = 0L;

    private readonly object _lock = new();

    public SnowflakeIdGenerator(long datacenterId, long machineId)
    {
        if (datacenterId < 0 || datacenterId > MaxDatacenterId)
            throw new ArgumentOutOfRangeException(nameof(datacenterId));

        if (machineId < 0 || machineId > MaxMachineId)
            throw new ArgumentOutOfRangeException(nameof(machineId));

        _datacenterId = datacenterId;
        _machineId = machineId;
    }

    public long NextId()
    {
        lock (_lock)
        {
            var timestamp = CurrentTimeMillis();

            // ⏰ Clock rollback protection
            if (timestamp < _lastTimestamp)
                throw new InvalidOperationException(
                    $"Clock moved backwards. Refusing for {_lastTimestamp - timestamp} ms");

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & MaxSequence;

                // Sequence exhausted → wait next millisecond
                if (_sequence == 0)
                    timestamp = WaitNextMillis(_lastTimestamp);
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;

            return
                ((timestamp - Epoch) << TimestampShift) |
                (_datacenterId << DatacenterIdShift) |
                (_machineId << MachineIdShift) |
                _sequence;
        }
    }

    private static long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = CurrentTimeMillis();
        while (timestamp <= lastTimestamp)
            timestamp = CurrentTimeMillis();
        return timestamp;
    }

    private static long CurrentTimeMillis()
        => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
