using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性的可绑定封装，实现属性值更改时通知客户端。
    /// </summary>
    /// <remarks><para>
    /// 设置数据绑定时，应绑定到 <see cref="Bindable{T}.Value"/>，而非实例本身。
    /// </para><para>
    /// 除初始化外，不应给此类的实例赋值，而应更改 <see cref="Bindable{T}.Value"/> 的值。
    /// </para><para>
    /// 直接更改实例的值将不会触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件，
    /// 并会替换 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件委托，破坏绑定关系。
    /// </para></remarks>
    /// <typeparam name="T"><see cref="Bindable{T}"/> 的值的类型。</typeparam>
    [Serializable]
    public class Bindable<T> : BindableObject
    {
        /// <summary>
        /// <see cref="Bindable{T}.Value"/> 的值。
        /// </summary>
        private T InternalValue;

        /// <summary>
        /// 使用默认值初始化 <see cref="Bindable{T}"/> 类的新实例。
        /// </summary>
        public Bindable() { }

        /// <summary>
        /// 使用指定的值初始化 <see cref="Bindable{T}"/> 类的新实例。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的值。</param>
        public Bindable(T value) => this.Value = value;

        /// <summary>
        /// 当前 <see cref="Bindable{T}"/> 实例的值。
        /// </summary>
        public T Value
        {
            get => this.InternalValue;
            set => this.SetProperty(ref this.InternalValue, value);
        }

        /// <summary>
        /// 返回表示当前实例的值的字符串。
        /// </summary>
        /// <returns><see cref="Bindable{T}.Value"/> 的等效字符串表达形式。</returns>
        public override string ToString() => this.Value?.ToString();

        /// <summary>
        /// 创建一个新的 <see cref="Bindable{T}"/> 对象，并将其初始化为指定的值。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的值。</param>
        /// <returns>使用 <paramref name="value"/> 初始化的 <see cref="Bindable{T}"/> 对象。</returns>
        public static implicit operator Bindable<T>(T value) => new Bindable<T>(value);

        /// <summary>
        /// 返回指定 <see cref="Bindable{T}"/> 对象的值。
        /// </summary>
        /// <param name="bindable">一个 <see cref="Bindable{T}"/> 对象。</param>
        /// <returns><paramref name="bindable"/> 的值。</returns>
        public static implicit operator T(Bindable<T> bindable) => bindable.Value;
    }
}
