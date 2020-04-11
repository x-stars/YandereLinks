using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性更改通知类型 <see cref="INotifyPropertyChanged"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ObservableDataObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 表示所有属性的值。
        /// </summary>
        private readonly ConcurrentDictionary<string, object> Properties;

        /// <summary>
        /// 初始化 <see cref="ObservableDataObject"/> 类的新实例。
        /// </summary>
        protected ObservableDataObject()
        {
            this.Properties = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// 在属性值更改时发生。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 获取指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">要获取值的属性的名称。</param>
        /// <returns>指定属性的值；若不存在，则为 <see langword="default"/>。</returns>
        protected T GetProperty<T>(
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
            this.Properties.TryGetValue(propertyName, out var value);
            return (value is T valueT) ? valueT : default(T);
        }

        /// <summary>
        /// 设置指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="value">属性的新值，一般为 <see langword="value"/>。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        protected virtual void SetProperty<T>(T value,
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
            this.Properties[propertyName] = (object)value;
            this.NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// 通知指定属性值已更改。
        /// </summary>
        /// <param name="propertyName">已更改属性的名称。</param>
        protected void NotifyPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="ObservableDataObject.PropertyChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="PropertyChangedEventArgs"/>。</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }
    }
}
