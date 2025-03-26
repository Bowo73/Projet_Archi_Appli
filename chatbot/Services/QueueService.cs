using chatbot.Models;

namespace chatbot.Services
{
    public class QueueService
    {
        private readonly Queue<ProductRequest> _queue = new();

        public void Enqueue(ProductRequest request) => _queue.Enqueue(request);
        public ProductRequest? Dequeue() => _queue.Count > 0 ? _queue.Dequeue() : null;
        public bool HasItems => _queue.Count > 0;
    }
}