using System;
using System.Threading;
using System.Windows.Input;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供命令 <see cref="ICommand"/> 的无参数的抽象基类。
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// <see cref="CommandBase.IsExecutable"/> 的值。
        /// </summary>
        private bool _IsExecutable = true;

        /// <summary>
        /// 创建当前命令的线程的同步上下文。
        /// </summary>
        private readonly SynchronizationContext InitSyncContext;

        /// <summary>
        /// 初始化 <see cref="CommandBase"/> 类的新实例。
        /// </summary>
        protected CommandBase()
        {
            this.InitSyncContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// 获取或设置此命令是否可在其当前状态下执行。
        /// </summary>
        protected bool IsExecutable
        {
            get => this._IsExecutable;
            set { this._IsExecutable = value; this.OnCanExecuteChanged(); }
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public virtual bool CanExecute() => this.IsExecutable;

        /// <summary>
        /// 引发 <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            if ((this.InitSyncContext is null) ||
                (this.InitSyncContext == SynchronizationContext.Current))
            {
                this.OnCanExecuteChanged(null);
            }
            else
            {
                this.InitSyncContext.Post(this.OnCanExecuteChanged, null);
            }
        }

        /// <summary>
        /// 引发 <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        /// <param name="unused">不使用此参数。</param>
        private void OnCanExecuteChanged(object unused)
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。不使用此参数。</param>
        void ICommand.Execute(object parameter) => this.Execute();

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。不使用此参数。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        bool ICommand.CanExecute(object parameter) => this.CanExecute();
    }

    /// <summary>
    /// 提供命令 <see cref="ICommand"/> 的有参数的抽象基类。
    /// </summary>
    /// <typeparam name="TParameter">命令使用的数据的类型。</typeparam>
    public abstract class CommandBase<TParameter> : ICommand
    {
        /// <summary>
        /// <see cref="CommandBase{TParameter}.IsExecutable"/> 的值。
        /// </summary>
        private bool _IsExecutable = true;

        /// <summary>
        /// 创建当前命令的线程的同步上下文。
        /// </summary>
        private readonly SynchronizationContext InitSyncContext;

        /// <summary>
        /// 初始化 <see cref="CommandBase{TParameter}"/> 类的新实例。
        /// </summary>
        protected CommandBase()
        {
            this.InitSyncContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// 获取或设置此命令是否可在其当前状态下执行。
        /// </summary>
        protected bool IsExecutable
        {
            get => this._IsExecutable;
            set { this._IsExecutable = value; this.OnCanExecuteChanged(); }
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        public abstract void Execute(TParameter parameter);

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public virtual bool CanExecute(TParameter parameter) => this.IsExecutable;

        /// <summary>
        /// 引发 <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            if ((this.InitSyncContext is null) ||
                (this.InitSyncContext == SynchronizationContext.Current))
            {
                this.OnCanExecuteChanged(null);
            }
            else
            {
                this.InitSyncContext.Post(this.OnCanExecuteChanged, null);
            }
        }

        /// <summary>
        /// 引发 <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        /// <param name="unused">不使用此参数。</param>
        private void OnCanExecuteChanged(object unused)
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="parameter"/> 不为 <typeparamref name="TParameter"/> 类型。</exception>
        void ICommand.Execute(object parameter) => this.Execute((TParameter)parameter);

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="parameter"/> 不为 <typeparamref name="TParameter"/> 类型。</exception>
        bool ICommand.CanExecute(object parameter) => this.CanExecute((TParameter)parameter);
    }
}
