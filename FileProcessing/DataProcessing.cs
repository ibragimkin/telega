namespace FileProcessing
{
    public class DataProcessing
    {
        /// <summary>
        /// Сортирует данные.
        /// </summary>
        /// <param name="list">Список, который нужно сортировать.</param>
        /// <param name="propertyName">Поле, по которому нужно сортировать.</param>
        public List<IceHill> Sort(List<IceHill> list, string propertyName)
        {
            int index = IceHill.FindIndex(propertyName);
            return list.OrderBy(iceHill => iceHill[index]).ToList();
        }

        /// <summary>
        /// Сортирует данные в обратную сторону.
        /// </summary>
        /// <param name="list">Список, который нужно сортировать.</param>
        /// <param name="propertyName">Поле, по которому нужно сортировать.</param>
        public List<IceHill> SortReverse(List<IceHill> list, string propertyName)
        {
            int index = IceHill.FindIndex(propertyName);
            return list.OrderByDescending(iceHill => iceHill[index]).ToList();
        }

        public string filtrationPropertyName = ""; // Поле, по которому нужно фильтровать в Filtration.

        /// <summary>
        /// Фильтрует данные.
        /// </summary>
        /// <param name="list">Список, который нужно фильтровать.</param>
        /// <param name="keyword">Слово, по которому нужно фильтровать.</param>
        /// <returns></returns>
        public List<IceHill> Filtration(List<IceHill> list, string keyword)
        {
            int index = IceHill.FindIndex(filtrationPropertyName);
            List<IceHill> iceHills = list.Where(hill => hill[index].Contains(keyword)).ToList();
            return iceHills;
        }
    }
}
