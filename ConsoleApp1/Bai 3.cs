using System;
using System.Diagnostics;

namespace Bai3_HangDoiVong
{
    // Lớp tự cài đặt Hàng đợi vòng (Circular Queue) bằng mảng tĩnh
    public class CircularQueue<T>
    {
        private T[] items;      // Mảng nội bộ lưu trữ dữ liệu
        private int front;      // Chỉ số của phần tử nằm ở đầu hàng đợi
        private int count;      // Số phần tử thực tế đang có trong hàng đợi

        // Thuộc tính lấy số phần tử hiện tại
        public int Count => count;

        // Thuộc tính lấy dung lượng tối đa của mảng hiện tại
        public int Capacity => items.Length;

        // Khởi tạo hàng đợi với dung lượng mặc định ban đầu là 4
        public CircularQueue(int initialCapacity = 4)
        {
            items = new T[initialCapacity];
            front = 0;
            count = 0;
        }

        // Thao tác thêm một phần tử vào cuối hàng đợi
        public void Enqueue(T value)
        {
            // Nếu số phần tử bằng dung lượng mảng thì mảng đã đầy, cần mở rộng gấp đôi
            if (count == items.Length)
            {
                MoRongMang();
            }

            // Vị trí thêm mới (rear) được tính xoay vòng từ front và số lượng count hiện có
            int rear = (front + count) % items.Length;
            items[rear] = value;

            // Tăng số lượng phần tử hiện tại lên 1
            count++;
        }

        // Thao tác lấy và xóa phần tử ở đầu hàng đợi
        public T Dequeue()
        {
            // Bắt buộc ném ngoại lệ nếu cố tình lấy ra khi hàng đợi đang rỗng
            if (count == 0)
            {
                throw new InvalidOperationException("Hàng đợi đang rỗng, không thể lấy phần tử!");
            }

            // Lấy giá trị tại vị trí đầu hàng (front)
            T value = items[front];

            // Gán ô vừa lấy về default (null hoặc 0) để bộ thu gom rác dọn dẹp bộ nhớ
            items[front] = default;

            // Dịch chuyển front sang ô kế tiếp theo công thức quay vòng tròn
            front = (front + 1) % items.Length;

            // Giảm số lượng phần tử hiện tại đi 1
            count--;

            return value;
        }

        // Thao tác xem phần tử ở đầu hàng đợi mà không xóa
        public T Peek()
        {
            // Không thể xem nếu hàng đợi rỗng
            if (count == 0)
            {
                throw new InvalidOperationException("Hàng đợi đang rỗng, không có phần tử để xem!");
            }

            return items[front];
        }

        // Hàm mở rộng mảng gấp đôi dung lượng khi bị đầy
        private void MoRongMang()
        {
            int oldCapacity = items.Length;
            int newCapacity = oldCapacity * 2;
            T[] newArray = new T[newCapacity];

            // Sao chép từng phần tử theo đúng thứ tự logic từ front đến cuối hàng
            for (int i = 0; i < count; i++)
            {
                // Chỉ số trong mảng cũ xoay vòng bắt đầu từ front
                newArray[i] = items[(front + i) % oldCapacity];
            }

            // Gán lại mảng mới và đặt lại chỉ số đầu hàng về 0 để dữ liệu liền mạch
            items = newArray;
            front = 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== CHƯƠNG TRÌNH KIỂM THỬ BÀI 3: HÀNG ĐỢI VÒNG (CIRCULAR QUEUE) ===\n");

            // STT 1: Hàng rỗng
            Console.WriteLine("--- [STT 1: Hàng rỗng] ---");
            var q1 = new CircularQueue<string>();
            Console.WriteLine($"Khởi tạo: Count = {q1.Count}, Capacity = {q1.Capacity}");
            try
            {
                q1.Dequeue();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Dequeue hàng rỗng: Ném ngoại lệ thành công ({e.Message})");
            }
            try
            {
                q1.Peek();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Peek hàng rỗng: Ném ngoại lệ thành công ({e.Message})\n");
            }

            // STT 2: Vừa đầy
            Console.WriteLine("--- [STT 2: Vừa đầy] ---");
            var q2 = new CircularQueue<string>();
            q2.Enqueue("A"); q2.Enqueue("B"); q2.Enqueue("C"); q2.Enqueue("D");
            Console.WriteLine($"Sau khi Enqueue 4 phần tử: Count = {q2.Count}, Capacity = {q2.Capacity}");
            Console.WriteLine($"Peek phần tử đầu: {q2.Peek()}\n");

            // STT 3: Quay vòng chỉ số, chưa mở rộng
            Console.WriteLine("--- [STT 3: Quay vòng chỉ số] ---");
            var q3 = new CircularQueue<string>();
            q3.Enqueue("A"); q3.Enqueue("B"); q3.Enqueue("C"); q3.Enqueue("D");
            Console.Write($"Lấy ra 2 lần đầu: {q3.Dequeue()}, {q3.Dequeue()}");
            q3.Enqueue("E"); q3.Enqueue("F");
            Console.WriteLine($" | Thêm E, F -> Capacity vẫn là: {q3.Capacity}");
            Console.Write("Lấy ra 4 lần tiếp: ");
            while (q3.Count > 0)
            {
                Console.Write(q3.Dequeue() + " ");
            }
            Console.WriteLine("\n");

            // STT 4: Mở rộng khi front = 0
            Console.WriteLine("--- [STT 4: Mở rộng khi front = 0] ---");
            var q4 = new CircularQueue<int>();
            for (int i = 1; i <= 5; i++) q4.Enqueue(i);
            Console.WriteLine($"Sau khi thêm 5 phần tử: Capacity mới = {q4.Capacity}");
            Console.Write("Lấy ra 5 lần: ");
            while (q4.Count > 0) Console.Write(q4.Dequeue() + " ");
            Console.WriteLine("\n");

            // STT 5: Mở rộng khi front khác 0
            Console.WriteLine("--- [STT 5: Mở rộng khi front khác 0] ---");
            var q5 = new CircularQueue<string>();
            q5.Enqueue("A"); q5.Enqueue("B"); q5.Enqueue("C"); q5.Enqueue("D");
            q5.Dequeue(); q5.Dequeue(); // Lấy A, B ra để front dịch sang vị trí 2
            q5.Enqueue("E"); q5.Enqueue("F"); q5.Enqueue("G"); // Kích hoạt mở rộng
            Console.WriteLine($"Capacity mới = {q5.Capacity}");
            Console.Write("Lấy ra các phần tử còn lại: ");
            while (q5.Count > 0) Console.Write(q5.Dequeue() + " ");
            Console.WriteLine("\n");

            // STT 6: Quay vòng 1.000.000 lần
            Console.WriteLine("--- [STT 6: Lặp 1 000 000 lần Enqueue rồi Dequeue] ---");
            var q6 = new CircularQueue<int>();
            for (int i = 0; i < 1000000; i++)
            {
                q6.Enqueue(i);
                q6.Dequeue();
            }
            Console.WriteLine($"Hoàn thành 1.000.000 lần lặp! Count = {q6.Count}, Capacity = {q6.Capacity}\n");

            // STT 7: Ca lớn 100.000 phần tử
            Console.WriteLine("--- [STT 7: Ca lớn thêm 100 000 phần tử] ---");
            var q7 = new CircularQueue<int>();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                q7.Enqueue(i);
            }
            sw.Stop();
            Console.WriteLine($"Đã thêm 100 000 phần tử. Capacity cuối: {q7.Capacity}");
            Console.WriteLine($"Thời gian chạy: {sw.ElapsedMilliseconds} ms (Yêu cầu: Capacity = 131072, < 1000 ms)");
        }
    }
}