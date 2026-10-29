using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Bai2_MoPhongQuayDichVu
{
    // Class lưu thông tin của một khách hàng
    public class Customer
    {
        public int Id { get; set; }           // Số thứ tự khách hàng (1, 2, 3...)
        public long Den { get; set; }         // Thời điểm đến (a)
        public long PhucVu { get; set; }      // Thời gian cần phục vụ (s)

        public Customer(int id, long den, long phucVu)
        {
            Id = id;
            Den = den;
            PhucVu = phucVu;
        }
    }

    class Program
    {
        // Hàm mô phỏng hệ thống quầy phục vụ
        // inChiTiet: true nếu in từng dòng khách (ca nhỏ), false nếu chỉ đo thời gian (ca lớn)
        public static void MoPhong(int k, List<(long a, long s)> danhSachKhach, bool inChiTiet = true)
        {
            // Dùng Queue để đảm bảo nguyên tắc FIFO: khách đến trước được phục vụ trước
            Queue<Customer> hangDoi = new Queue<Customer>();

            // Nạp toàn bộ khách vào hàng đợi theo thứ tự đến
            for (int i = 0; i < danhSachKhach.Count; i++)
            {
                hangDoi.Enqueue(new Customer(i + 1, danhSachKhach[i].a, danhSachKhach[i].s));
            }

            // Mảng lưu thời điểm rảnh tiếp theo của k quầy (ban đầu tất cả đều rảnh ở mốc 0)
            long[] thoiDiemRanh = new long[k];

            // Dùng kiểu long để tránh tràn số vì tổng thời gian chờ có thể vượt qua 2 tỷ (giới hạn int)
            long tongThoiGianCho = 0;
            long choLauNhat = 0;
            long hoanThanhLuc = 0;
            int soKhach = hangDoi.Count;

            // Xử lý lần lượt từng khách cho đến khi hàng đợi rỗng
            while (hangDoi.Count > 0)
            {
                // Dequeue lấy khách đầu tiên ra khỏi hàng với độ phức tạp O(1)
                Customer kh = hangDoi.Dequeue();

                // Quét qua k quầy để tìm quầy rảnh sớm nhất (chi phí Θ(k))
                int quayChon = 0;
                long ranhSomNhat = thoiDiemRanh[0];
                for (int q = 1; q < k; q++)
                {
                    // Dùng dấu < để nếu nhiều quầy cùng rảnh thì luôn giữ quầy có số thứ tự nhỏ nhất
                    if (thoiDiemRanh[q] < ranhSomNhat)
                    {
                        ranhSomNhat = thoiDiemRanh[q];
                        quayChon = q;
                    }
                }

                // Nếu quầy rảnh trước khi khách đến thì khách vào làm ngay; ngược lại khách phải chờ
                long batDau = Math.Max(kh.Den, ranhSomNhat);
                long cho = batDau - kh.Den;
                long xong = batDau + kh.PhucVu;

                // Cập nhật lại mốc thời gian quầy này sẽ rảnh tiếp
                thoiDiemRanh[quayChon] = xong;

                // Cộng dồn thống kê thời gian
                tongThoiGianCho += cho;
                if (cho > choLauNhat) choLauNhat = cho;
                if (xong > hoanThanhLuc) hoanThanhLuc = xong;

                // In chi tiết từng khách theo đúng định dạng mẫu của đề bài (quầy đánh số từ 1)
                if (inChiTiet)
                {
                    Console.WriteLine($"KH {kh.Id}: den {kh.Den}, quay {quayChon + 1}, bat dau {batDau}, cho {cho}, xong {xong}");
                }
            }

            // Tính thời gian chờ trung bình và in bằng CultureInfo.InvariantCulture để lấy dấu chấm thập phân
            double choTrungBinh = (double)tongThoiGianCho / soKhach;
            Console.WriteLine($"Cho trung binh: {choTrungBinh.ToString("0.00", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Cho lau nhat: {choLauNhat}");
            Console.WriteLine($"Hoan thanh luc: {hoanThanhLuc}\n");
        }

        static void Main2(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== CHƯƠNG TRÌNH KIỂM THỬ BÀI 2: MÔ PHỎNG QUẦY DỊCH VỤ ===\n");

            // STT 1: Một quầy
            Console.WriteLine("--- [STT 1: k = 1 quầy] ---");
            MoPhong(1, new List<(long, long)> { (0, 5), (1, 3), (2, 4) });

            // STT 2: Hai quầy
            Console.WriteLine("--- [STT 2: k = 2 quầy] ---");
            MoPhong(2, new List<(long, long)> { (0, 5), (1, 3), (2, 4) });

            // STT 3: Đến cùng lúc
            Console.WriteLine("--- [STT 3: Đến cùng lúc (k = 2 quầy)] ---");
            MoPhong(2, new List<(long, long)> { (0, 3), (0, 3), (0, 3) });

            // STT 4: Quầy rảnh rồi mới có khách đến
            Console.WriteLine("--- [STT 4: Quầy rảnh rồi khách mới đến (k = 1 quầy)] ---");
            MoPhong(1, new List<(long, long)> { (0, 2), (10, 3) });

            // STT 5: n = 1, k lớn hơn n
            Console.WriteLine("--- [STT 5: n = 1, k = 3 quầy] ---");
            MoPhong(3, new List<(long, long)> { (4, 6) });

            // STT 6: Ca lớn 100 000 khách, k = 10 quầy
            Console.WriteLine("--- [STT 6: Ca lớn 100 000 khách, k = 10 quầy] ---");
            var caLon = new List<(long, long)>(100000);
            for (int i = 1; i <= 100000; i++)
            {
                caLon.Add((i - 1, 10)); // Khách thứ i đến lúc i - 1, phục vụ 10
            }

            Stopwatch sw = Stopwatch.StartNew();
            // Không in chi tiết từng dòng để tránh nghẽn luồng Console
            MoPhong(10, caLon, inChiTiet: false);
            sw.Stop();

            Console.WriteLine($"Thời gian chạy ca lớn: {sw.ElapsedMilliseconds} ms (Yêu cầu: < 1000 ms)");
        }
    }
}