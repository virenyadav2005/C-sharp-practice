﻿// See https://aka.ms/new-console-template for more information

namespace DemoApp
{
    //Interface Seqregation principle :- like IProduct only for product data, IPaymentprocessor is only for payment and INotificationService is only for notifications 
    public interface IProduct
    {
        string Name { get; }
        decimal Price { get; }
    }

    //Open for extension closed for modification like we can add another method of payment like :- PhonePay,Internet Banking
    //LSV(Liskov Substitution Principle):- IPaymentProcessor can be used interchangeably with their base types.
    public interface IPaymentProcessor
    {
        void ProcessPayment(decimal amount);
    }

    
    public interface ICreditCardPaymentProcessor : IPaymentProcessor { }
    public interface IPaytmPaymentProcessor : IPaymentProcessor { }
    public interface IUPIPaymentProcessor : IPaymentProcessor { }


    public class CreditCardPaymentProcessor : ICreditCardPaymentProcessor
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of ${amount}");
        }
    }

    public class PaytmPaymentProcessor : IPaytmPaymentProcessor
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing Paytm payment of ${amount}");
        }
    }

    public class UPIPaymentProcessor : IUPIPaymentProcessor
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing payment using UPI of ${amount}");
        }
    }

    
    //Open for extension closed for modification like we can add another method of sending notification like :- WhatsApp
    //LSV(Liskov Substitution Principle):- INotificationService can be used interchangeably with their base types.
    public interface INotificationService
    {
        public void SendNotification(string message);
    }

    
    public class EmailNotificationService : INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"Sending email notification : {message}");
        }
    }

    
    public class SMSNotificationService : INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"Sending SMS notification : {message}");
        }
    }


// Single Responsibility principle
    public class Product : IProduct
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Product(string name , decimal price)
        {
            Name = name;
            Price = price;
        }

    }



// Single Responsiblity principle
    public class Order
    {
        public IProduct Product { get; private set; }
        public decimal TotalAmount => Product.Price;

        //Order depends on IPaymentProcessor and INotificationService not on concrete implementations.
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly INotificationService _notificationService;

        public Order(IProduct product ,IPaymentProcessor paymentProcessor, INotificationService notificationService)
        {
            Product = product;
            _paymentProcessor = paymentProcessor;
            _notificationService = notificationService;
        }


        public void PlaceOrder()
        {
            _paymentProcessor.ProcessPayment(TotalAmount);
            _notificationService.SendNotification($"Order placed for product: {Product.Name}");
        }

    }



    public class Program
    {
        static void Main(string[] args)
        {
            IProduct product = new Product("Laptop", 999.99m);

            IPaymentProcessor paymentProcessor = new CreditCardPaymentProcessor();
            INotificationService notificationService = new EmailNotificationService();

            Order orderService = new Order(product,paymentProcessor, notificationService);

            orderService.PlaceOrder();

            Console.WriteLine("Order processed successfully.");
        }
    }
}
