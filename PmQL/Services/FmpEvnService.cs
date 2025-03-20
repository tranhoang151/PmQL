using Microsoft.EntityFrameworkCore;
using PmQL.Data;
using PmQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PmQL.Services
{
    public class FmpEvnService : IFmpEvnService
    {
        private readonly PmQlContext _context;

        public FmpEvnService(PmQlContext context)
        {
            _context = context;
        }

        public static double CustomRound(double value)
        {
            return (value > -1 && value < 1) ? 0 : Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        public async Task CalculateAndSaveFmpEvnAsync()
        {
            var smpData = await _context.NhậpGiáNms
                .Where(n => n.Giá == "SMP")
                .ToListAsync();
            var canData = await _context.NhậpGiáNms
                .Where(n => n.Giá == "CAN")
                .ToListAsync();

            var fmpEvnList = new List<FmpEvn>();

            foreach (var smp in smpData)
            {
                var can = canData.FirstOrDefault(c => c.Ngày == smp.Ngày);
                if (can != null)
                {
                    var fmpEvn = new FmpEvn
                    {
                        Ngay = (int?)smp.Ngày,
                        Gia = "FMP",
                        ChuKy0h30 = smp._0h30 + can._0h30,
                        ChuKy1h = smp._1h + can._1h,
                        ChuKy1h30 = smp._1h30 + can._1h30,
                        ChuKy2h = smp._2h + can._2h,
                        ChuKy2h30 = smp._2h30 + can._2h30,
                        ChuKy3h = smp._3h + can._3h,
                        ChuKy3h30 = smp._3h30 + can._3h30,
                        ChuKy4h = smp._4h + can._4h,
                        ChuKy4h30 = smp._4h30 + can._4h30,
                        ChuKy5h = smp._5h + can._5h,
                        ChuKy5h30 = smp._5h30 + can._5h30,
                        ChuKy6h = smp._6h + can._6h,
                        ChuKy6h30 = smp._6h30 + can._6h30,
                        ChuKy7h = smp._7h + can._7h,
                        ChuKy7h30 = smp._7h30 + can._7h30,
                        ChuKy8h = smp._8h + can._8h,
                        ChuKy8h30 = smp._8h30 + can._8h30,
                        ChuKy9h = smp._9h + can._9h,
                        ChuKy9h30 = smp._9h30 + can._9h30,
                        ChuKy10h = smp._10h + can._10h,
                        ChuKy10h30 = smp._10h30 + can._10h30,
                        ChuKy11h = smp._11h + can._11h,
                        ChuKy11h30 = smp._11h30 + can._11h30,
                        ChuKy12h = smp._12h + can._12h,
                        ChuKy12h30 = smp._12h30 + can._12h30,
                        ChuKy13h = smp._13h + can._13h,
                        ChuKy13h30 = smp._13h30 + can._13h30,
                        ChuKy14h = smp._14h + can._14h,
                        ChuKy14h30 = smp._14h30 + can._14h30,
                        ChuKy15h = smp._15h + can._15h,
                        ChuKy15h30 = smp._15h30 + can._15h30,
                        ChuKy16h = smp._16h + can._16h,
                        ChuKy16h30 = smp._16h30 + can._16h30,
                        ChuKy17h = smp._17h + can._17h,
                        ChuKy17h30 = smp._17h30 + can._17h30,
                        ChuKy18h = smp._18h + can._18h,
                        ChuKy18h30 = smp._18h30 + can._18h30,
                        ChuKy19h = smp._19h + can._19h,
                        ChuKy19h30 = smp._19h30 + can._19h30,
                        ChuKy20h = smp._20h + can._20h,
                        ChuKy20h30 = smp._20h30 + can._20h30,
                        ChuKy21h = smp._21h + can._21h,
                        ChuKy21h30 = smp._21h30 + can._21h30,
                        ChuKy22h = smp._22h + can._22h,
                        ChuKy22h30 = smp._22h30 + can._22h30,
                        ChuKy23h = smp._23h + can._23h,
                        ChuKy23h30 = smp._23h30 + can._23h30,
                        ChuKy24h = smp._24h + can._24h,
                        Tong = smp.Tổng + can.Tổng
                    };
                    fmpEvnList.Add(fmpEvn);
                }
            }

            _context.FmpEvns.RemoveRange(_context.FmpEvns);
            await _context.FmpEvns.AddRangeAsync(fmpEvnList);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FmpEvn>> GetFmpEvnDataAsync()
        {
            return await _context.FmpEvns.ToListAsync();
        }

        public async Task CalculateAndSavePmEvnAsync()
        {
            var fmpEvnData = await _context.FmpEvns.ToListAsync();
            var kData = await _context.NhậpGiáNms
                .Where(n => n.Giá == "k")
                .ToListAsync();

            var pmEvnList = new List<PmEvn>();

            foreach (var fmpEvn in fmpEvnData)
            {
                var k = kData.FirstOrDefault(k => k.Ngày == fmpEvn.Ngay);
                if (k != null)
                {
                    var pmEvn = new PmEvn
                    {
                        Ngay = fmpEvn.Ngay,
                        Gia = "Pm",
                        ChuKy0h30 = Math.Round(fmpEvn.ChuKy0h30.GetValueOrDefault() * k._0h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy1h = Math.Round(fmpEvn.ChuKy1h.GetValueOrDefault() * k._1h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy1h30 = Math.Round(fmpEvn.ChuKy1h30.GetValueOrDefault() * k._1h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy2h = Math.Round(fmpEvn.ChuKy2h.GetValueOrDefault() * k._2h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy2h30 = Math.Round(fmpEvn.ChuKy2h30.GetValueOrDefault() * k._2h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy3h = Math.Round(fmpEvn.ChuKy3h.GetValueOrDefault() * k._3h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy3h30 = Math.Round(fmpEvn.ChuKy3h30.GetValueOrDefault() * k._3h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy4h = Math.Round(fmpEvn.ChuKy4h.GetValueOrDefault() * k._4h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy4h30 = Math.Round(fmpEvn.ChuKy4h30.GetValueOrDefault() * k._4h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy5h = Math.Round(fmpEvn.ChuKy5h.GetValueOrDefault() * k._5h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy5h30 = Math.Round(fmpEvn.ChuKy5h30.GetValueOrDefault() * k._5h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy6h = Math.Round(fmpEvn.ChuKy6h.GetValueOrDefault() * k._6h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy6h30 = Math.Round(fmpEvn.ChuKy6h30.GetValueOrDefault() * k._6h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy7h = Math.Round(fmpEvn.ChuKy7h.GetValueOrDefault() * k._7h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy7h30 = Math.Round(fmpEvn.ChuKy7h30.GetValueOrDefault() * k._7h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy8h = Math.Round(fmpEvn.ChuKy8h.GetValueOrDefault() * k._8h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy8h30 = Math.Round(fmpEvn.ChuKy8h30.GetValueOrDefault() * k._8h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy9h = Math.Round(fmpEvn.ChuKy9h.GetValueOrDefault() * k._9h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy9h30 = Math.Round(fmpEvn.ChuKy9h30.GetValueOrDefault() * k._9h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy10h = Math.Round(fmpEvn.ChuKy10h.GetValueOrDefault() * k._10h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy10h30 = Math.Round(fmpEvn.ChuKy10h30.GetValueOrDefault() * k._10h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy11h = Math.Round(fmpEvn.ChuKy11h.GetValueOrDefault() * k._11h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy11h30 = Math.Round(fmpEvn.ChuKy11h30.GetValueOrDefault() * k._11h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy12h = Math.Round(fmpEvn.ChuKy12h.GetValueOrDefault() * k._12h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy12h30 = Math.Round(fmpEvn.ChuKy12h30.GetValueOrDefault() * k._12h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy13h = Math.Round(fmpEvn.ChuKy13h.GetValueOrDefault() * k._13h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy13h30 = Math.Round(fmpEvn.ChuKy13h30.GetValueOrDefault() * k._13h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy14h = Math.Round(fmpEvn.ChuKy14h.GetValueOrDefault() * k._14h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy14h30 = Math.Round(fmpEvn.ChuKy14h30.GetValueOrDefault() * k._14h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy15h = Math.Round(fmpEvn.ChuKy15h.GetValueOrDefault() * k._15h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy15h30 = Math.Round(fmpEvn.ChuKy15h30.GetValueOrDefault() * k._15h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy16h = Math.Round(fmpEvn.ChuKy16h.GetValueOrDefault() * k._16h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy16h30 = Math.Round(fmpEvn.ChuKy16h30.GetValueOrDefault() * k._16h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy17h = Math.Round(fmpEvn.ChuKy17h.GetValueOrDefault() * k._17h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy17h30 = Math.Round(fmpEvn.ChuKy17h30.GetValueOrDefault() * k._17h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy18h = Math.Round(fmpEvn.ChuKy18h.GetValueOrDefault() * k._18h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy18h30 = Math.Round(fmpEvn.ChuKy18h30.GetValueOrDefault() * k._18h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy19h = Math.Round(fmpEvn.ChuKy19h.GetValueOrDefault() * k._19h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy19h30 = Math.Round(fmpEvn.ChuKy19h30.GetValueOrDefault() * k._19h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy20h = Math.Round(fmpEvn.ChuKy20h.GetValueOrDefault() * k._20h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy20h30 = Math.Round(fmpEvn.ChuKy20h30.GetValueOrDefault() * k._20h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy21h = Math.Round(fmpEvn.ChuKy21h.GetValueOrDefault() * k._21h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy21h30 = Math.Round(fmpEvn.ChuKy21h30.GetValueOrDefault() * k._21h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy22h = Math.Round(fmpEvn.ChuKy22h.GetValueOrDefault() * k._22h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy22h30 = Math.Round(fmpEvn.ChuKy22h30.GetValueOrDefault() * k._22h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy23h = Math.Round(fmpEvn.ChuKy23h.GetValueOrDefault() * k._23h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy23h30 = Math.Round(fmpEvn.ChuKy23h30.GetValueOrDefault() * k._23h30.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        ChuKy24h = Math.Round(fmpEvn.ChuKy24h.GetValueOrDefault() * k._24h.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero),
                        Tong = Math.Round(new double?[]
                        {
                            fmpEvn.ChuKy0h30 * k._0h30, fmpEvn.ChuKy1h * k._1h, fmpEvn.ChuKy1h30 * k._1h30,
                            fmpEvn.ChuKy2h * k._2h, fmpEvn.ChuKy2h30 * k._2h30, fmpEvn.ChuKy3h * k._3h,
                            fmpEvn.ChuKy3h30 * k._3h30, fmpEvn.ChuKy4h * k._4h, fmpEvn.ChuKy4h30 * k._4h30,
                            fmpEvn.ChuKy5h * k._5h, fmpEvn.ChuKy5h30 * k._5h30, fmpEvn.ChuKy6h * k._6h,
                            fmpEvn.ChuKy6h30 * k._6h30, fmpEvn.ChuKy7h * k._7h, fmpEvn.ChuKy7h30 * k._7h30,
                            fmpEvn.ChuKy8h * k._8h, fmpEvn.ChuKy8h30 * k._8h30, fmpEvn.ChuKy9h * k._9h,
                            fmpEvn.ChuKy9h30 * k._9h30, fmpEvn.ChuKy10h * k._10h, fmpEvn.ChuKy10h30 * k._10h30,
                            fmpEvn.ChuKy11h * k._11h, fmpEvn.ChuKy11h30 * k._11h30, fmpEvn.ChuKy12h * k._12h,
                            fmpEvn.ChuKy12h30 * k._12h30, fmpEvn.ChuKy13h * k._13h, fmpEvn.ChuKy13h30 * k._13h30,
                            fmpEvn.ChuKy14h * k._14h, fmpEvn.ChuKy14h30 * k._14h30, fmpEvn.ChuKy15h * k._15h,
                            fmpEvn.ChuKy15h30 * k._15h30, fmpEvn.ChuKy16h * k._16h, fmpEvn.ChuKy16h30 * k._16h30,
                            fmpEvn.ChuKy17h * k._17h, fmpEvn.ChuKy17h30 * k._17h30, fmpEvn.ChuKy18h * k._18h,
                            fmpEvn.ChuKy18h30 * k._18h30, fmpEvn.ChuKy19h * k._19h, fmpEvn.ChuKy19h30 * k._19h30,
                            fmpEvn.ChuKy20h * k._20h, fmpEvn.ChuKy20h30 * k._20h30, fmpEvn.ChuKy21h * k._21h,
                            fmpEvn.ChuKy21h30 * k._21h30, fmpEvn.ChuKy22h * k._22h, fmpEvn.ChuKy22h30 * k._22h30,
                            fmpEvn.ChuKy23h * k._23h, fmpEvn.ChuKy23h30 * k._23h30, fmpEvn.ChuKy24h * k._24h
                        }.Where(x => x.HasValue).Sum().GetValueOrDefault(), 0, MidpointRounding.AwayFromZero)
                    };
                    pmEvnList.Add(pmEvn);
                }
            }

            _context.PmEvns.RemoveRange(_context.PmEvns);
            await _context.PmEvns.AddRangeAsync(pmEvnList);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PmEvn>> GetPmEvnDataAsync()
        {
            return await _context.PmEvns.ToListAsync();
        }

        public async Task CalculateAndSaveFmpSaiAsync()
        {
            var fmpEvnData = await _context.FmpEvns.ToListAsync();
            var fmpA0Data = await _context.FmpA0s.ToListAsync();

            var fmpSaiList = new List<FmpSai>();

            foreach (var fmpEvn in fmpEvnData)
            {
                var fmpA0 = fmpA0Data.FirstOrDefault(a => a.Ngày == fmpEvn.Ngay);
                if (fmpA0 != null)
                {
                    var fmpSai = new FmpSai
                    {
                        Ngay = fmpEvn.Ngay,
                        //Gia = "EVN vs A0",
                        ChuKy0h30 = fmpEvn.ChuKy0h30 - fmpA0._0h30,
                        ChuKy1h = fmpEvn.ChuKy1h - fmpA0._1h,
                        ChuKy1h30 = fmpEvn.ChuKy1h30 - fmpA0._1h30,
                        ChuKy2h = fmpEvn.ChuKy2h - fmpA0._2h,
                        ChuKy2h30 = fmpEvn.ChuKy2h30 - fmpA0._2h30,
                        ChuKy3h = fmpEvn.ChuKy3h - fmpA0._3h,
                        ChuKy3h30 = fmpEvn.ChuKy3h30 - fmpA0._3h30,
                        ChuKy4h = fmpEvn.ChuKy4h - fmpA0._4h,
                        ChuKy4h30 = fmpEvn.ChuKy4h30 - fmpA0._4h30,
                        ChuKy5h = fmpEvn.ChuKy5h - fmpA0._5h,
                        ChuKy5h30 = fmpEvn.ChuKy5h30 - fmpA0._5h30,
                        ChuKy6h = fmpEvn.ChuKy6h - fmpA0._6h,
                        ChuKy6h30 = fmpEvn.ChuKy6h30 - fmpA0._6h30,
                        ChuKy7h = fmpEvn.ChuKy7h - fmpA0._7h,
                        ChuKy7h30 = fmpEvn.ChuKy7h30 - fmpA0._7h30,
                        ChuKy8h = fmpEvn.ChuKy8h - fmpA0._8h,
                        ChuKy8h30 = fmpEvn.ChuKy8h30 - fmpA0._8h30,
                        ChuKy9h = fmpEvn.ChuKy9h - fmpA0._9h,
                        ChuKy9h30 = fmpEvn.ChuKy9h30 - fmpA0._9h30,
                        ChuKy10h = fmpEvn.ChuKy10h - fmpA0._10h,
                        ChuKy10h30 = fmpEvn.ChuKy10h30 - fmpA0._10h30,
                        ChuKy11h = fmpEvn.ChuKy11h - fmpA0._11h,
                        ChuKy11h30 = fmpEvn.ChuKy11h30 - fmpA0._11h30,
                        ChuKy12h = fmpEvn.ChuKy12h - fmpA0._12h,
                        ChuKy12h30 = fmpEvn.ChuKy12h30 - fmpA0._12h30,
                        ChuKy13h = fmpEvn.ChuKy13h - fmpA0._13h,
                        ChuKy13h30 = fmpEvn.ChuKy13h30 - fmpA0._13h30,
                        ChuKy14h = fmpEvn.ChuKy14h - fmpA0._14h,
                        ChuKy14h30 = fmpEvn.ChuKy14h30 - fmpA0._14h30,
                        ChuKy15h = fmpEvn.ChuKy15h - fmpA0._15h,
                        ChuKy15h30 = fmpEvn.ChuKy15h30 - fmpA0._15h30,
                        ChuKy16h = fmpEvn.ChuKy16h - fmpA0._16h,
                        ChuKy16h30 = fmpEvn.ChuKy16h30 - fmpA0._16h30,
                        ChuKy17h = fmpEvn.ChuKy17h - fmpA0._17h,
                        ChuKy17h30 = fmpEvn.ChuKy17h30 - fmpA0._17h30,
                        ChuKy18h = fmpEvn.ChuKy18h - fmpA0._18h,
                        ChuKy18h30 = fmpEvn.ChuKy18h30 - fmpA0._18h30,
                        ChuKy19h = fmpEvn.ChuKy19h - fmpA0._19h,
                        ChuKy19h30 = fmpEvn.ChuKy19h30 - fmpA0._19h30,
                        ChuKy20h = fmpEvn.ChuKy20h - fmpA0._20h,
                        ChuKy20h30 = fmpEvn.ChuKy20h30 - fmpA0._20h30,
                        ChuKy21h = fmpEvn.ChuKy21h - fmpA0._21h,
                        ChuKy21h30 = fmpEvn.ChuKy21h30 - fmpA0._21h30,
                        ChuKy22h = fmpEvn.ChuKy22h - fmpA0._22h,
                        ChuKy22h30 = fmpEvn.ChuKy22h30 - fmpA0._22h30,
                        ChuKy23h = fmpEvn.ChuKy23h - fmpA0._23h,
                        ChuKy23h30 = fmpEvn.ChuKy23h30 - fmpA0._23h30,
                        ChuKy24h = fmpEvn.ChuKy24h - fmpA0._24h,
                        Tong = fmpEvn.Tong - fmpA0.Tổng
                    };
                    fmpSaiList.Add(fmpSai);
                }
            }

            _context.FmpSais.RemoveRange(_context.FmpSais);
            await _context.FmpSais.AddRangeAsync(fmpSaiList);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FmpSai>> GetFmpSaiDataAsync()
        {
            return await _context.FmpSais.ToListAsync();
        }

        public async Task CalculateAndSavePmSaiAsync()
        {
            // Lấy dữ liệu từ Pm_EVN và Pm_A0
            var pmEvnData = await _context.PmEvns.ToListAsync();
            var pmA0Data = await _context.PmA0s.ToListAsync();

            var pmSaiList = new List<PmSai>();

            foreach (var pmEvn in pmEvnData)
            {
                var pmA0 = pmA0Data.FirstOrDefault(a => a.Ngày == pmEvn.Ngay && a.Giá == pmEvn.Gia);
                if (pmA0 != null)
                {
                    var pmSai = new PmSai
                    {
                        Ngay = pmEvn.Ngay,
                        Gia = pmEvn.Gia,
                        ChuKy0h30 = CalculateDifference(pmEvn.ChuKy0h30, pmA0._0h30),
                        ChuKy1h = CalculateDifference(pmEvn.ChuKy1h, pmA0._1h),
                        ChuKy1h30 = CalculateDifference(pmEvn.ChuKy1h30, pmA0._1h30),
                        ChuKy2h = CalculateDifference(pmEvn.ChuKy2h, pmA0._2h),
                        ChuKy2h30 = CalculateDifference(pmEvn.ChuKy2h30, pmA0._2h30),
                        ChuKy3h = CalculateDifference(pmEvn.ChuKy3h, pmA0._3h),
                        ChuKy3h30 = CalculateDifference(pmEvn.ChuKy3h30, pmA0._3h30),
                        ChuKy4h = CalculateDifference(pmEvn.ChuKy4h, pmA0._4h),
                        ChuKy4h30 = CalculateDifference(pmEvn.ChuKy4h30, pmA0._4h30),
                        ChuKy5h = CalculateDifference(pmEvn.ChuKy5h, pmA0._5h),
                        ChuKy5h30 = CalculateDifference(pmEvn.ChuKy5h30, pmA0._5h30),
                        ChuKy6h = CalculateDifference(pmEvn.ChuKy6h, pmA0._6h),
                        ChuKy6h30 = CalculateDifference(pmEvn.ChuKy6h30, pmA0._6h30),
                        ChuKy7h = CalculateDifference(pmEvn.ChuKy7h, pmA0._7h),
                        ChuKy7h30 = CalculateDifference(pmEvn.ChuKy7h30, pmA0._7h30),
                        ChuKy8h = CalculateDifference(pmEvn.ChuKy8h, pmA0._8h),
                        ChuKy8h30 = CalculateDifference(pmEvn.ChuKy8h30, pmA0._8h30),
                        ChuKy9h = CalculateDifference(pmEvn.ChuKy9h, pmA0._9h),
                        ChuKy9h30 = CalculateDifference(pmEvn.ChuKy9h30, pmA0._9h30),
                        ChuKy10h = CalculateDifference(pmEvn.ChuKy10h, pmA0._10h),
                        ChuKy10h30 = CalculateDifference(pmEvn.ChuKy10h30, pmA0._10h30),
                        ChuKy11h = CalculateDifference(pmEvn.ChuKy11h, pmA0._11h),
                        ChuKy11h30 = CalculateDifference(pmEvn.ChuKy11h30, pmA0._11h30),
                        ChuKy12h = CalculateDifference(pmEvn.ChuKy12h, pmA0._12h),
                        ChuKy12h30 = CalculateDifference(pmEvn.ChuKy12h30, pmA0._12h30),
                        ChuKy13h = CalculateDifference(pmEvn.ChuKy13h, pmA0._13h),
                        ChuKy13h30 = CalculateDifference(pmEvn.ChuKy13h30, pmA0._13h30),
                        ChuKy14h = CalculateDifference(pmEvn.ChuKy14h, pmA0._14h),
                        ChuKy14h30 = CalculateDifference(pmEvn.ChuKy14h30, pmA0._14h30),
                        ChuKy15h = CalculateDifference(pmEvn.ChuKy15h, pmA0._15h),
                        ChuKy15h30 = CalculateDifference(pmEvn.ChuKy15h30, pmA0._15h30),
                        ChuKy16h = CalculateDifference(pmEvn.ChuKy16h, pmA0._16h),
                        ChuKy16h30 = CalculateDifference(pmEvn.ChuKy16h30, pmA0._16h30),
                        ChuKy17h = CalculateDifference(pmEvn.ChuKy17h, pmA0._17h),
                        ChuKy17h30 = CalculateDifference(pmEvn.ChuKy17h30, pmA0._17h30),
                        ChuKy18h = CalculateDifference(pmEvn.ChuKy18h, pmA0._18h),
                        ChuKy18h30 = CalculateDifference(pmEvn.ChuKy18h30, pmA0._18h30),
                        ChuKy19h = CalculateDifference(pmEvn.ChuKy19h, pmA0._19h),
                        ChuKy19h30 = CalculateDifference(pmEvn.ChuKy19h30, pmA0._19h30),
                        ChuKy20h = CalculateDifference(pmEvn.ChuKy20h, pmA0._20h),
                        ChuKy20h30 = CalculateDifference(pmEvn.ChuKy20h30, pmA0._20h30),
                        ChuKy21h = CalculateDifference(pmEvn.ChuKy21h, pmA0._21h),
                        ChuKy21h30 = CalculateDifference(pmEvn.ChuKy21h30, pmA0._21h30),
                        ChuKy22h = CalculateDifference(pmEvn.ChuKy22h, pmA0._22h),
                        ChuKy22h30 = CalculateDifference(pmEvn.ChuKy22h30, pmA0._22h30),
                        ChuKy23h = CalculateDifference(pmEvn.ChuKy23h, pmA0._23h),
                        ChuKy23h30 = CalculateDifference(pmEvn.ChuKy23h30, pmA0._23h30),
                        ChuKy24h = CalculateDifference(pmEvn.ChuKy24h, pmA0._24h),
                    };

                    // Tính tổng của 48 chu kỳ
                    pmSai.Tong = new double?[]
                    {
                        pmSai.ChuKy0h30, pmSai.ChuKy1h, pmSai.ChuKy1h30, pmSai.ChuKy2h, pmSai.ChuKy2h30,
                        pmSai.ChuKy3h, pmSai.ChuKy3h30, pmSai.ChuKy4h, pmSai.ChuKy4h30, pmSai.ChuKy5h,
                        pmSai.ChuKy5h30, pmSai.ChuKy6h, pmSai.ChuKy6h30, pmSai.ChuKy7h, pmSai.ChuKy7h30,
                        pmSai.ChuKy8h, pmSai.ChuKy8h30, pmSai.ChuKy9h, pmSai.ChuKy9h30, pmSai.ChuKy10h,
                        pmSai.ChuKy10h30, pmSai.ChuKy11h, pmSai.ChuKy11h30, pmSai.ChuKy12h, pmSai.ChuKy12h30,
                        pmSai.ChuKy13h, pmSai.ChuKy13h30, pmSai.ChuKy14h, pmSai.ChuKy14h30, pmSai.ChuKy15h,
                        pmSai.ChuKy15h30, pmSai.ChuKy16h, pmSai.ChuKy16h30, pmSai.ChuKy17h, pmSai.ChuKy17h30,
                        pmSai.ChuKy18h, pmSai.ChuKy18h30, pmSai.ChuKy19h, pmSai.ChuKy19h30, pmSai.ChuKy20h,
                        pmSai.ChuKy20h30, pmSai.ChuKy21h, pmSai.ChuKy21h30, pmSai.ChuKy22h, pmSai.ChuKy22h30,
                        pmSai.ChuKy23h, pmSai.ChuKy23h30, pmSai.ChuKy24h
                    }.Where(x => x.HasValue).Sum().GetValueOrDefault();

                    pmSaiList.Add(pmSai);
                }
            }

            // Xóa dữ liệu cũ trong Pm_Sai và lưu dữ liệu mới
            _context.PmSais.RemoveRange(_context.PmSais);
            await _context.PmSais.AddRangeAsync(pmSaiList);
            await _context.SaveChangesAsync();
        }

        //Tính chênh lệch và làm tròn
        private double? CalculateDifference(double? pmEvnValue, double? pmA0Value)
        {
            if (!pmEvnValue.HasValue || !pmA0Value.HasValue) return null;

            var diff = pmEvnValue.Value - pmA0Value.Value;
            return (diff >= -0.9 && diff <= 0.9) ? 0 : Math.Round(diff, 0, MidpointRounding.AwayFromZero);
        }

        public async Task<List<PmSai>> GetPmSaiDataAsync()
        {
            return await _context.PmSais.ToListAsync();
        }
    }
}