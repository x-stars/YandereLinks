using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性更改通知类型 <see cref="INotifyPropertyChanged"/> 基于数据存储的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ObservableDataObject : ObservableObject
    {
        /// <summary>
        /// 所有属性的数据存储。
        /// </summary>
        private readonly ConcurrentDictionary<string, object> Properties =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 初始化 <see cref="ObservableDataObject"/> 类的新实例。
        /// </summary>
        protected ObservableDataObject() { }

        /// <summary>
        /// 获取指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">属性的名称，可由编译器自动获取。</param>
        /// <returns>指定属性的值；若不存在，则为 <see langword="default"/>。</returns>
        protected T GetProperty<T>(
            [CallerMemberName] string propertyName = null)
        {
            this.Properties.TryGetValue(propertyName, out var oValue);
            return (oValue is T value) ? value : default(T);
        }

        /// <summary>
        /// 设置指定属性的值，并通知属性值已更改。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="value">属性的新值，一般为 <see langword="value"/>。</param>
        /// <param name="propertyName">属性的名称，可由编译器自动获取。</param>
        protected void SetProperty<T>(T value,
            [CallerMemberName] string propertyName = null)
        {
            this.Properties[propertyName] = (object)value;
            this.NotifyPropertyChanged(propertyName);
        }
    }
}
