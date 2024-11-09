namespace Library
{
    #region TestZone

    internal class MainApp
    {
        private static void Main(string[] args)
        {
        }
    }

    #endregion

    #region Method

    public static class LidraryMethod
    {
        #region ICollection

        /// <summary>
        /// IColliction 타입에 value가 중복되는지 확인하고 추가합니다.
        /// </summary>
        /// <param name="value">추가할 값입니다.</param>
        /// <returns>추가에 성공하면 true, 실패하면 false를 반환합니다.</returns>
        public static bool TryAdd<T>(this ICollection<T> range, T value)
        {
            if (range.Contains(value))
                return false;

            range.Add(value);
            return true;
        }

        #endregion

        #region Dictionary

        /// <summary>
        /// 딕셔너리에 주어진 키와 값을 추가하거나, 키가 이미 존재할 경우 값을 업데이트합니다.
        /// </summary>
        /// <remarks>
        /// 키가 이미 존재하면 기존 값을 새 값으로 대체하며, 존재하지 않으면 새 키와 값을 추가합니다.
        /// </remarks>
        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dict, K key, V value)
            => dict[key] = value;

        /// <summary>
        /// 주어진 값에 대한 첫 번째 키를 찾습니다.
        /// </summary>
        /// <param name="dict">값을 검색할 딕셔너리</param>
        /// <param name="val">찾고자 하는 값</param>
        /// <returns>
        /// 찾은 키를 반환합니다. 값이 존재하지 않을 경우, 기본값(<typeparamref name="K"/>)을 반환합니다.
        /// </returns>
        /// <remarks>
        /// 이 메서드는 값이 중복된 경우 첫 번째로 발견된 키를 반환합니다.
        /// </remarks>
        public static K? FindFirstKeyByValue<K, V>(this Dictionary<K, V> dict, V val)
            => dict.FirstOrDefault(entry => EqualityComparer<V>.Default.Equals(entry.Value, val)).Key;

        /// <summary>
        /// 주어진 딕셔너리를 반전시킵니다. 각 키와 값을 교환하여 새로운 딕셔너리를 생성합니다.
        /// </summary>
        /// <param name="dict">반전시킬 원본 딕셔너리</param>
        /// <returns>키와 값이 반전된 새 딕셔너리</returns>
        /// <remarks>
        /// 원본 딕셔너리에서 값이 고유한 경우에만 유효하게 작동합니다.
        /// 값이 중복되면 마지막 값이 키로 저장됩니다.
        /// </remarks>
        public static Dictionary<V, K> Invert<K, V>(this Dictionary<K, V> dict)
            => dict.ToDictionary(valuePair => valuePair.Value, valuePair => valuePair.Key);

        /// <summary>
        /// 두 개의 딕셔너리를 병합합니다.
        /// </summary>
        /// <param name="target">병합될 대상 딕셔너리</param>
        /// <param name="adder">병합할 딕셔너리</param>
        /// <param name="firstTarget">
        /// true인 경우, 키가 중복될 때 대상 딕셔너리의 값을 유지합니다.
        /// false인 경우, 추가할 딕셔너리의 값으로 덮어씁니다.
        /// </param>
        public static void MergeWith<K, V>(this Dictionary<K, V> target, Dictionary<K, V> adder, bool firstTarget = true)
        {
            foreach (K kValue in adder.Keys)
                target[kValue] = firstTarget && target.ContainsKey(kValue) ? target[kValue] : adder[kValue];

            target.Where(pair => EqualityComparer<V>.Default.Equals(pair.Value, pair.Value)).Select(pair => pair.Key).ToList();
        }

        /// <summary>
        /// 지정된 값과 일치하는 값이 있는 모든 키-값을 딕셔너리에서 제거합니다.
        /// </summary>
        /// <param name="value">제거할 값입니다.</param>
        /// <remarks>
        /// 이 메서드는 지정된 값과 일치하는 키와 해당 키의 값을 딕셔너리에서 모두 제거합니다.
        /// </remarks>
        public static void DeleteByValue<K, V>(this Dictionary<K, V> dict, V value)
            => dict.Where(pair => EqualityComparer<V>.Default.Equals(pair.Value, value)).Select(pair => pair.Key).ToList().ForEach(key => dict.Remove(key));

        /// <summary>
        /// 딕셔너리에서 지정된 값과 일치하는 모든 값을 새 값으로 변경합니다.
        /// </summary>
        /// <param name="valueForChange">변경할 기존 값입니다.</param>
        /// <param name="value">새로 설정할 값입니다.</param>
        /// <remarks>
        /// 이 메서드는 딕셔너리에서 <paramref name="valueForChange"/>와 일치하는 값을 가진 항목을 찾아 해당 값을 <paramref name="value"/>로 업데이트합니다.
        /// </remarks>
        public static void ChangeValueTo<K, V>(this Dictionary<K, V> dict, V valueForChage, V value)
            => dict.Where(pair => EqualityComparer<V>.Default.Equals(pair.Value, valueForChage)).Select(pair => pair.Key).ToList().ForEach(key => dict[key] = value);

        /// <summary>
        /// 주어진 값과 일치하는 모든 키-값 쌍을 찾아 새로운 딕셔너리로 반환합니다.
        /// </summary>
        /// <param name="findValue">찾고자 하는 값입니다.</param>
        /// <returns>값이 일치하는 모든 키, 값을 포함하는 새로운 딕셔너리</returns>
        public static Dictionary<K, V> GetOverlapValue<K, V>(this Dictionary<K, V> dict, V findValue)
            => dict.Where(pair => EqualityComparer<V>.Default.Equals(pair.Value, findValue)).ToDictionary(pair => pair.Key, pair => pair.Value) ?? new Dictionary<K, V>();

        #endregion
    }

    #endregion

    #region Class

    #region NotOverlapValue
    /// <summary>
    /// T값을 넣어 범위를 지정하여 범위안의 T값을 가져옵니다.
    /// </summary>
    public class NotOverlapValue<T>
    {
        public event Action? OnReset;

        private Random _random;

        private List<T> _range = new List<T>();
        private List<T> _values = new List<T>();

        public NotOverlapValue(IEnumerable<T> range, Random? random = null)
        {
            _range.AddRange(range);
            _values.AddRange(range);
            _random = random ?? new Random();
        }

        public NotOverlapValue(T range, Random? random = null) : this(new[] { range }, random) { }

        public NotOverlapValue(Random? random = null) : this(Enumerable.Empty<T>(), random) { }

        /// <summary>
        /// 초기화 시 기준이 될 범위의 수를 가져옵니다.
        /// </summary>
        public int GetRangeCount() => _range.Count;

        /// <summary>
        /// 초기회 되기 까지 남은 값들의 수를 가져옵니다.
        /// </summary>
        public int GetValueCount() => _values.Count;

        /// <summary>
        /// 값(<typeparamref name="T"/>)을 범위에 추가합니다.
        /// </summary>
        /// <param name="value">추가할 값입니다.</param>
        /// <remarks>
        /// 값을 추가할 수 없는 경우에 대한 예외 처리가 포함되어 있지 않습니다.
        /// </remarks>
        public void AddRange(T range)
        {
            if (range == null || _range.Contains(range)) return;
            _range.Add(range);
        }

        /// <summary>
        /// 지정된 값의 컬렉션을 범위에 추가합니다.
        /// </summary>
        /// <param name="value">추가할 값의 컬렉션입니다.</param>
        /// <remarks>
        /// null 값에 대한 예외 처리가 포함되어 있지 않습니다.
        /// </remarks>
        public void AddRange(IEnumerable<T> range)
        {
            if (range == null) return;

            foreach (T entry in range)
                if (!_range.Contains(entry))
                    _range.Add(entry);
        }

        /// <summary>
        /// 값(<typeparamref name="T"/>)을 후에 나올 값에 추가합니다.
        /// 추가된 값은 초기화 되지 않고 사라집니다.
        /// </summary>
        /// <param name="value">추가할 값입니다.</param>
        /// <remarks>
        /// 값을 추가할 수 없는 경우에 대한 예외 처리가 포함되어 있지 않습니다.
        /// </remarks>
        public void AddValue(T value)
        {
            if (value == null || _range.Contains(value)) return;
            _values.Add(value);
        }

        /// <summary>
        /// 지정된 값의 컬렉션을 후에 나올 값에 추가합니다.
        /// 추가된 값은 초기화 되지 않고 사라집니다.
        /// </summary>
        /// <param name="value">추가할 값의 컬렉션입니다.</param>
        /// <remarks>
        /// null 값에 대한 예외 처리가 포함되어 있지 않습니다.
        /// </remarks>
        public void AddValue(IEnumerable<T> value)
        {
            if (value == null) return;

            foreach (T entry in value)
                if (!_values.Contains(entry))
                    _values.Add(entry);
        }

        /// <summary>
        /// 범위의 값(<typeparamref name="T"/>)을 삭제합니다.
        /// </summary>
        public void RangeRemove(T value) => _range.Remove(value);

        /// <summary>
        /// n번째 값(<typeparamref name="T"/>)을 범위에서 삭제합니다.
        /// </summary>
        public void RangeRemoveAt(int n)
        {
            if (n >= 0 && n < _range.Count)
                _range.RemoveAt(n);
        }

        /// <summary>
        /// 범위를 초기화합니다.
        /// </summary>
        public void RangeClear() => _range.Clear();

        /// <summary>
        /// 범위의 값 전체을 가져옵니다.
        /// </summary>
        /// <returns>
        /// 값들(<typeparamref name="T"/>)을 반환합니다.
        /// </returns>
        public IEnumerable<T> GetRange() => _range;

        /// <summary>
        /// 값의 범위를 초기화합니다.
        /// </summary>
        public void Reset()
        {
            _values = new List<T>(_range);
            OnReset?.Invoke();
        }

        /// <summary>
        /// 렌덤으로 범위 안의 T값을 가져옵니다.
        /// 한 범위가 다 지나기 전까지 같은 값을 가져오지 않습니다.
        /// 단, 중복된 범위는 예외입니다.
        /// </summary>
        /// <returns>범위가 비어 있을 시 기본값(<typeparamref name="T"/>)을 반환합니다.</returns>
        public T GetValue()
        {
            if (_values.Count <= 0)
                Reset();

            if (_values.Count <= 0)
                return default(T);

            int randNum = _random.Next(0, _values.Count);
            T randValue = _values[randNum];
            _values.RemoveAt(randNum);
            return randValue;
        }

        /// <summary>
        /// 범위 안의 값(<typeparamref name="T"/>)을 렌덤으로 가져옵니다.
        /// </summary>
        /// <returns>값(<typeparamref name="T"/>)을 반환합니다.</returns>
        public T GetRangeInValue()
            => _range[_random.Next(0, _range.Count)];


        /// <summary>
        /// 지정된 인덱스에 해당하는 값을 반환합니다.
        /// </summary>
        /// <param name="n">가져올 값의 인덱스입니다.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 인덱스가 범위를 벗어나는 경우 발생합니다.
        /// </exception>
        /// <remarks>
        /// 0 이상의 인덱스만 허용되며, 해당 범위 내의 인덱스여야 합니다.
        /// </remarks>
        public T GetValue(int n)
        {
            if (n < 0 || n >= _range.Count)
                throw new ArgumentOutOfRangeException("해당 매개변수에 해당하는 인덱스 값이 없습니다.");

            T randValue = _values[n];
            _values.RemoveAt(n);
            return randValue;
        }

        /// <summary>
        /// 지정된 인덱스에 해당하는 범위 안의 값을 반환합니다.
        /// </summary>
        /// <param name="n">가져올 값의 인덱스입니다.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 인덱스가 범위를 벗어나는 경우 발생합니다.
        /// </exception>
        /// <remarks>
        /// 0 이상의 인덱스만 허용되며, 해당 범위 내의 인덱스여야 합니다.
        /// </remarks>
        public T GetRangeInValue(int n)
        {
            if (n < 0 || n >= _range.Count)
                throw new ArgumentOutOfRangeException("해당 매개변수에 해당하는 인덱스 값이 없습니다.");

            return _range[n];
        }
    }
    #endregion

    #endregion
}