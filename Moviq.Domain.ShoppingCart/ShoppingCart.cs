using Moviq.Domain.Products;
using Moviq.Interfaces;
using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cart
{
    public class ShoppingCart
    {
        private readonly IAmountCalculator _amountCalculator;
        private readonly IPaymentHandler _paymentHandler;
        private readonly List<Product> _cartItems;
        public readonly ICartItemServices CartItem;
        public string cartId;

        public ICartState CartState { get; set; }
        public ShoppingCart(IAmountCalculator amountCalculator, IPaymentHandler paymentHandler, ICartState cartState, ICartItemServices CartItemServices)
        {
            ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
            cartId = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
            CartState = cartState;
            _amountCalculator = amountCalculator;
            _paymentHandler = paymentHandler;
            _cartItems = new List<Product>();
            CartItem = CartItemServices;
          
        }

        public void Add(Product cartItem)
        {
            CartState.AddItem(this, cartItem);
        }


        public void Remove(Product cartItem)
        {
            CartState.RemoveItem(this, cartItem);
        }

        public void Pay()
        {
            CartState.Pay(this, _amountCalculator, _paymentHandler);
        }

        public void EmptyCart(ShoppingCart cart)
        {
            CartState.EmptyCart(cart);
        }
    }

    public interface ICartState
    {
        void AddItem(ShoppingCart cart, Product item);
        void RemoveItem(ShoppingCart cart, Product item);
        bool Pay(ShoppingCart cart, IAmountCalculator amountCalculator, IPaymentHandler paymentHandler);
        void EmptyCart(ShoppingCart cart);
    }

    public class EmptyState : ICartState
    {
        CartItem CartItem = new CartItem();

        public void AddItem(ShoppingCart cart, Product item)
        {
            //databse add;
            CartItem.CartId = cart.cartId;
            CartItem.Uid = item.Uid;
            CartItem.Title = item.Title;
            CartItem.Price = item.Price;
            cart.CartItem.AddCartItem(CartItem);
            cart.CartState = new ActiveState();
        }

        public void RemoveItem(ShoppingCart cart, Product item)
        {
            //throw an error
        }

        public bool Pay(ShoppingCart cart, IAmountCalculator amountCalculator, IPaymentHandler paymentHandler)
        {
            return false;
            //throw an error
        }

        public void EmptyCart(ShoppingCart cart)
        {
            //throw error
        }
    }

    public class ActiveState : ICartState
    {
        CartItem CartItem = new CartItem();

        public void AddItem(ShoppingCart cart, Product item)
        {
            CartItem.Uid = item.Uid;
            CartItem.Title = item.Title;
            CartItem.Price = item.Price;
            cart.CartItem.AddCartItem(CartItem);
            cart.CartState = new ActiveState();
        }

        public void RemoveItem(ShoppingCart cart, Product item)
        {
            CartItem.CartId = cart.cartId;
            CartItem.Uid = item.Uid;
            CartItem.Title = item.Title;
            CartItem.Price = item.Price;
            cart.CartItem.UpdateCartItem(CartItem);
            cart.CartState = new ActiveState();
        }

        public bool Pay(ShoppingCart cart, IAmountCalculator amountCalculator, IPaymentHandler paymentHandler)
        {
           // decimal amountToPay = amountCalculator.CaluculatePaymentAmount(cart.CartItems);
           // bool paymentStatus = paymentHandler.Pay(amountToPay);
            //cart.CartState = new PaidState();
            return false;
        }
        public void EmptyCart(ShoppingCart cart)
        {
            cart.CartItem.UpdateCartItem(CartItem);
        }
    }

    public class PaidState : ICartState
    {
        public void AddItem(ShoppingCart cart, Product item)
        {
            //throw an Error
        }

        public void RemoveItem(ShoppingCart cart, Product item)
        {
            //throw an Error
        }

        public bool Pay(ShoppingCart cart, IAmountCalculator amountCalculator, IPaymentHandler paymentHandler)
        {
            return false;
            //throw an Error
        }
        public void EmptyCart(ShoppingCart cart)
        {
            //throw error
        }
    }


    public interface IAmountCalculator
    {
        decimal CaluculatePaymentAmount(IEnumerable<Product> items);
    }

    public class AmountCalculator : IAmountCalculator
    {
       
        public decimal CaluculatePaymentAmount(IEnumerable<Product> items)
        {
            return 0;
        }
    }

    public interface IPaymentHandler
    {
        bool Pay(decimal amount);
    }

    public class PaymentHandler : IPaymentHandler
    {

        public bool Pay(decimal amount)
        {
            return true;
        }
    }
}
