public class SimpleTimer
{
    private readonly float _timeInterval;
    private float _timer;

    private readonly int _maxCount;
    private int _count;

    public SimpleTimer(float timeInterval, int maxCount = -1)
    {
        _timeInterval = timeInterval;
        _maxCount = maxCount;
        _timer = 0;
        _count = 0;
    }

    public bool UpdateCheck(float deltaTime)
    {
        if (_maxCount > 0 && _count == _maxCount)
        {
            return false;
        }

        _timer += deltaTime;
        if (_timer < _timeInterval)
        {
            return false;
        }

        _timer -= _timeInterval;
        _count++;
        return true;
    }
}