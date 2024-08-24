using System;
using System.Windows.Input;

namespace Memo.Commands
{
    // ICommand : UI 에서 명령 처리 하기 위한 표준 ( true 실행 가능, false 실행 불가 )
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object>? _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // 명령 실행 가능 여부 판단 메서드
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // 명령이 실행될 때 호출 되는 메서드
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        // CanExecute 메서드의 반환값이 변경될 때 발생. UI 갱신용
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
