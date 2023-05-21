using System;
using System.Collections.Generic;
using System.Threading;

namespace RefactoringGuru.DesignPatterns.Observer.Conceptual
{
    public interface IObserver
    {
        // Untuk menerima upadate
        void Update(ISubject subject);
    }

    public interface ISubject
    {
        // Lampirkan pengamat ke subjek.
        void Attach(IObserver observer);

        // Lepaskan pengamat dari subjek.
        void Detach(IObserver observer);

        // Beri tahu semua pengamat tentang suatu peristiwa.
        void Notify();
    }

    // Subjek memiliki beberapa status penting dan memberi tahu pengamat saat
    // perubahan status.
    public class Subject : ISubject
    {
        // Demi kesederhanaan, keadaan Subjek, penting bagi semua
        // pelanggan, disimpan dalam variabel ini.
        public int State { get; set; } = -0;

        // Daftar pelanggan. Dalam kehidupan nyata, daftar pelanggan bisa jadi
        // disimpan lebih komprehensif (dikategorikan berdasarkan jenis acara, dll.).
        private List<IObserver> _observers = new List<IObserver>();

        // Metode manajemen langganan.
        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Attached an observer.");
            this._observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer);
            Console.WriteLine("Subject: Detached an observer.");
        }

        // Pemicu pembaruan di setiap pelanggan.
        public void Notify()
        {
            Console.WriteLine("Subject: Notifying observers...");

            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        // Biasanya, logika langganan hanya sebagian kecil dari Subjek
        // benar-benar bisa melakukannya. Subjek umumnya memegang beberapa logika bisnis penting,
        // yang memicu metode notifikasi setiap kali ada sesuatu yang penting
        // akan terjadi (atau setelahnya).
        public void SomeBusinessLogic()
        {
            Console.WriteLine("\nSubject: I'm doing something important.");
            this.State = new Random().Next(0, 10);

            Thread.Sleep(15);

            Console.WriteLine("Subject: My state has just changed to: " + this.State);
            this.Notify();
        }
    }

    // Pengamat Konkrit bereaksi terhadap pembaruan yang dikeluarkan oleh Subjek yang mereka miliki
    // telah dilampirkan.
    class ConcreteObserverA : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State < 3)
            {
                Console.WriteLine("ConcreteObserverA: Reacted to the event.");
            }
        }
    }

    class ConcreteObserverB : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State == 0 || (subject as Subject).State >= 2)
            {
                Console.WriteLine("ConcreteObserverB: Reacted to the event.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Kodingan Client
            var subject = new Subject();
            var observerA = new ConcreteObserverA();
            subject.Attach(observerA);

            var observerB = new ConcreteObserverB();
            subject.Attach(observerB);

            subject.SomeBusinessLogic();
            subject.SomeBusinessLogic();

            subject.Detach(observerB);

            subject.SomeBusinessLogic();
        }
    }
}