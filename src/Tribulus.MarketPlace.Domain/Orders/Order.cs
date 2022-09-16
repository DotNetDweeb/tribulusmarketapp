using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribulus.MarketPlace.Orders.Events;
using Tribulus.MarketPlace.Products.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Tribulus.MarketPlace.Orders
{
    public class Order:FullAuditedAggregateRoot<Guid>
    {
        public Guid OwnerUserId { get; private set; }

        public string Name { get; private set; } //Needs Null Or WhiteSpace Validation

        public decimal TotalValue { get; private set; } //Needs Validation

        public OrderState State { get; private set; } //Changed during PlaceOrder();, Might also need to change it with another method

        public ICollection<OrderItem> OrderItems { get; private set; } // added with AddOrderItems()


        private Order()
        {
        }

        public Order(Guid id, Guid ownerUserId, string name) : base(id)
        {
            OwnerUserId = ownerUserId;
            State = OrderState.Pending;
            UpdateName(name);
            OrderItems = new List<OrderItem>();
        }
        public Order UpdateName(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Name = name;
            return this;
        }

        public Order AddOrderItem(
            Guid orderItemId,
            Guid productId,
            decimal price,
            int quantity)
        {

            var newOrderItem = new OrderItem(orderItemId, Id, productId, price, quantity);
            OrderItems.Add(newOrderItem);
            return this;
        }
        public Order UpdateOrderItemQuantity(Guid orderItemId, int quantity)
        {
            CheckIfOrderItemExists(orderItemId);
            var orderItem = OrderItems.First(o => o.Id == orderItemId);
            orderItem.UpdateQuantity(quantity);
            return this;
        }

        public Order RemoveOrderItem(Guid orderItemId)
        {
            CheckIfOrderItemExists(orderItemId);
            var orderItem = OrderItems.First(o => o.Id == orderItemId);
            OrderItems.Remove(orderItem);
            return this;
        }
        private void CheckIfOrderItemExists(Guid orderItemId)
        {
            if (!OrderItems.Any(o => o.Id == orderItemId))
                throw new ArgumentNullException(nameof(orderItemId));
        }
        internal void PlaceOrder()
        {
            State = OrderState.Confirmed;
            //ADD an EVENT TO BE PUBLISHED
            //Added event because it shouldn't leak data from Order Bounded Context to Product Bounded Context
            AddLocalEvent(
                new PlaceOrderEventData
                {
                    Order = this
                }
            );
        }

    }
}
