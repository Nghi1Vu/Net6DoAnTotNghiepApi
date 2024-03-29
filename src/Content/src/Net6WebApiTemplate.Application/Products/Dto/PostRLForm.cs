﻿using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class PostRLForm
    {
        public int UserId { get; set; }
        public int SemesterID { get; set; }
        public List<RLFormDetail> detail { get; set; }

      
    }
    public class RLFormDetail
    {
        public int RLAnswerID { get; set; }
        public int Score { get; set; }
    }
}
