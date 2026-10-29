using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bai1_KiemTraDauNgoac
{
    class Bai1
    {
        // Hàm kiểm tra chuỗi, trả về -1 nếu hợp lệ, ngược lại trả về vị trí lỗi
        static int KiemTraNgoac(string s)
        {
            // Dùng Stack để lưu các cặp (ký tự ngoặc, vị trí xuất hiện)
            // LIFO giúp ngoặc mở vào sau sẽ được lấy ra ghép đôi trước
            Stack<(char KyTu, int ViTri)> stack = new Stack<(char KyTu, int ViTri)>();

            // Duyệt từng ký tự từ đầu đến cuối chuỗi
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                // Nếu là dấu ngoặc mở thì cất vào Stack
                if (c == '(' || c == '[' || c == '{')
                {
                    stack.Push((c, i));
                }
                // Nếu là dấu ngoặc đóng
                else if (c == ')' || c == ']' || c == '}')
                {
                    // Lỗi 1: Gặp ngoặc đóng mà Stack không còn gì để ghép (đóng thừa)
                    if (stack.Count == 0)
                    {
                        return i;
                    }

                    // Lấy ngoặc mở trên đỉnh ra xem có khớp loại không
                    var dinh = stack.Pop();

                    // Lỗi 2: Đóng sai loại (ví dụ mở '[' mà đóng ')')
                    if (c == ')' && dinh.KyTu != '(') return i;
                    if (c == ']' && dinh.KyTu != '[') return i;
                    if (c == '}' && dinh.KyTu != '{') return i;
                }
                // Các chữ, số hoặc phép toán khác thì bỏ qua, không làm gì cả
            }

            // Lỗi 3: Duyệt hết chữ rồi mà Stack vẫn còn (ngoặc mở không được đóng)
            if (stack.Count > 0)
            {
                // Rút hết các phần tử ra để tìm đến cái nằm ở đáy (vào sớm nhất)
                var phanTuDay = stack.Pop();
                while (stack.Count > 0)
                {
                    phanTuDay = stack.Pop();
                }
                return phanTuDay.ViTri;
            }

            // Nếu không vướng lỗi nào thì chuỗi hoàn toàn hợp lệ
            return -1;
        }

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Thử một vài chuỗi mẫu trong đề bài
            ChayThu("{(a+b)×[c−d]}"); // Trường hợp đúng
            ChayThu("{(a+b]×c)");     // Lỗi đóng sai loại ở vị trí 5
            ChayThu("(a+b");          // Lỗi mở chưa đóng ở vị trí 0
            ChayThu("a+b)");          // Lỗi đóng thừa ở vị trí 3
            ChayThu("");              // Chuỗi rỗng -> Hợp lệ
            ChayThu("([)]");          // Đan xen -> Sai ở vị trí 2
            ChayThu("{[(");           // Sai ở vị trí 0 (ngoặc sớm nhất chưa đóng)

            // Kiểm tra ca lớn 1.000.000 ký tự bằng Stopwatch theo yêu cầu đề bài
            Console.WriteLine("\n--- KIỂM TRA CA LỚN (1 000 000 ký tự) ---");
            string caLon = new string('(', 500000) + new string(')', 500000);
            Stopwatch sw = Stopwatch.StartNew();
            int ketQua = KiemTraNgoac(caLon);
            sw.Stop();

            if (ketQua == -1)
                Console.WriteLine($"Ca 1.000.000 ký tự: HOP LE (Thời gian: {sw.ElapsedMilliseconds} ms)");
            else
                Console.WriteLine($"Ca 1.000.000 ký tự: SAI tai vi tri {ketQua} (Thời gian: {sw.ElapsedMilliseconds} ms)");
        }

        // Hàm phụ in kết quả ra màn hình cho gọn gàng
        static void ChayThu(string s)
        {
            int kq = KiemTraNgoac(s);
            if (kq == -1)
            {
                Console.WriteLine($"Chuỗi \"{s}\" -> HOP LE");
            }
            else
            {
                Console.WriteLine($"Chuỗi \"{s}\" -> SAI tai vi tri {kq}");
            }
        }
    }
}