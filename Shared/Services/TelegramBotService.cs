using InstagramCommerce.Shared.Data;
using InstagramCommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace InstagramCommerce.Shared.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly OrderService _orderService;
        private readonly InstagramCommerceContext _context;

        public TelegramBotService(OrderService orderService, InstagramCommerceContext context)
        {
            _orderService = orderService;
            _context = context;
            _botClient = new TelegramBotClient("6539772057:AAHxg3M3SACb-mU6pGrZI6J2WwXmhCndZW8"); // Replace with your bot token
        }

        public void Start()
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Telegram Bot Error: {exception.Message}");
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                if (messageText == "/start")
                {
                    await botClient.SendTextMessageAsync(chatId, "Welcome to InstagramCommerce bot! Use /products to see the product list or /order <product_id> to place an order.", cancellationToken: cancellationToken);
                }
                else if (messageText == "/products")
                {
                    var products = await _context.Products.ToListAsync();
                    foreach (var product in products)
                    {
                        await botClient.SendTextMessageAsync(chatId, $"{product.Id}: {product.Name} - {product.Description} - ${product.Price}", cancellationToken: cancellationToken);
                    }
                }
                else if (messageText.StartsWith("/order"))
                {
                    var parts = messageText.Split(' ');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int productId))
                    {
                        var product = await _context.Products.FindAsync(productId);
                        if (product != null)
                        {
                            var order = new Order
                            {
                                ProductId = productId,
                                Status = "New",
                                // You might want to add more order details here
                            };
                            await _orderService.PlaceOrderAsync(order);
                            await botClient.SendTextMessageAsync(chatId, $"Order placed for product {product.Name}.", cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId, "Invalid product ID.", cancellationToken: cancellationToken);
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, "Usage: /order <product_id>", cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Unknown command. Use /products to see the product list or /order <product_id> to place an order.", cancellationToken: cancellationToken);
                }
            }
        }
    }
}
