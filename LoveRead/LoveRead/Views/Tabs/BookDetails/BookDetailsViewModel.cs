﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.ViewModel;
using LoveRead.Views.Tabs.ReadBook;
using LoveRead.Views.Tabs.SaveBook;
using MaterialDesignThemes.Wpf.Transitions;

namespace LoveRead.Views.Tabs.BookDetails
{
    public partial class BookDetailsViewModel : BaseViewModel
    {
        private readonly IMessangerService _messanger;

        public RelayCommand MoveNextCommand
            => new RelayCommand(MoveNext);

        public RelayCommand MoveBackCommand
            => new RelayCommand(MoveBack);

        public BookDetailsViewModel(IMessangerService messanger)
        {
            _messanger = messanger;

            Messenger.Default.Register<NotificationMessage<TabSwitchMessange>>(this, ProcessTabSwitchMessange);
        }

        private void MoveNext()
        {
            _messanger.NotifyTabSwitch(this, nameof(SaveBookViewModel), new TabSwitchMessange { Data = Book });
            Transitioner.MoveNextCommand.Execute(null, BookDetailsView);
        }

        private void MoveBack()
        {
            _messanger.NotifyTabSwitch(this, nameof(ReadBookViewModel), new TabSwitchMessange());
            Transitioner.MovePreviousCommand.Execute(null, BookDetailsView);
        }

        private void ProcessTabSwitchMessange(NotificationMessage<TabSwitchMessange> message)
        {
            if (!Equals(GetType().Name, message.Target))
                return;

            var book = (WebBook) message.Content.Data;
            if (!(Book is null) && Book.Id == book.Id)
                return;

            Book = book;
            GetBookDetails();
        }

        private void GetBookDetails()
        {
            BookName = Book.Name;
            BookAuthor = Book.Author;
            BookLogo = Book.ImageUrl;
            BookPagesCount = Book.PagesCount;
        }
    }
}
