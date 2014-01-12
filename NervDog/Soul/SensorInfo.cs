using System;
using Box2D.XNA;

namespace NervDog.Soul
{
    public delegate void SensorHandler(Unit sensorUnit, Unit sensedUnit);

    public class SensorInfo : IDisposable
    {
        private readonly int _max;
        private readonly Unit[] _sensedUnits;
        private int _count;
        private bool _enable = true;
        private Fixture _fixture;
        private Unit _unit;
        private string _sensorName;

        public SensorInfo(Unit unit, Fixture fixture, int maxSense)
        {
            _unit = unit;
            _fixture = fixture;
            _fixture.SetUserData(this);
            _max = maxSense;
            _sensedUnits = new Unit[maxSense];
            _count = 0;
        }

        public int SensedCount
        {
            get { return _count; }
        }

        public string SensorName
        {
            set { _sensorName = value; }
            get { return _sensorName; }
        }

        public bool Enable
        {
            set
            {
                _enable = value;
                if (value == false)
                {
                    Clear();
                }
            }
            get { return _enable; }
        }

        public Unit SensorUnit
        {
            get { return _unit; }
        }

        public bool IsDisposed
        {
            get { return (_fixture == null); }
        }

        public void Dispose()
        {
            if (_fixture != null)
            {
                _unit.Body.DestroyFixture(_fixture);
                _fixture = null;
                _unit = null;
            }
        }

        public event SensorHandler UnitEnter = null;
        public event SensorHandler UnitLeave = null;

        public bool Add(Unit unit)
        {
            if (_count < _max)
            {
                _sensedUnits[_count] = unit;
                _count++;
                if (UnitEnter != null)
                {
                    UnitEnter(_unit, unit);
                }
                return true;
            }
            return false;
        }

        public bool Remove(Unit unit)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_sensedUnits[i] == unit)
                {
                    int last = _count - 1;
                    if (i != last)
                    {
                        _sensedUnits[i] = _sensedUnits[_count - 1];
                    }
                    _count--;

                    if (UnitLeave != null)
                    {
                        UnitLeave(_unit, unit);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Contains(Unit unit)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_sensedUnits[i] == unit)
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            _count = 0;
        }

        public Unit GetSensedUnit(int index)
        {
            return _sensedUnits[index];
        }

        public Unit[] SensedUnits
        {
            get { return _sensedUnits; }
        }
    }
}