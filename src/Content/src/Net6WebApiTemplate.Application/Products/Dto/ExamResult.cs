﻿using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class ExamResult
    {
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int IndependentClassID { get; set; }
        public int ModulesID { get; set; }
        public string ModulesCode { get; set; }
        public string fullname { get; set; }
        public string Usercode { get; set; }
        public string Class { get; set; }
        public string PrintCode { get; set; }
        public string ModulesName { get; set; }
        public string SemesterIndex { get; set; }
        public decimal Credits { get; set; }
        public decimal? SGKL1 { get; set; }
        public decimal? TBKTTK { get; set; }
        public decimal? EXAM { get; set; }
        public decimal? ScoreFinal { get; set; }
        public decimal? D4 { get; set; }
        public string XH { get; set; }
        public string SymbolName { get; set; }
    }
    public class ExamByClass
    {
        public string Usercode { get; set; }
        public string ModulesName { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string Fullname { get; set; }
        public int UserID { get; set; }
        public int ModulesID { get; set; }
        public decimal? Score1 { get; set; }
        public decimal Credits { get; set; }
        public decimal? Score2 { get; set; }

    }
}
