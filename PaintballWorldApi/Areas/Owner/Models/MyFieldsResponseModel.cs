﻿using PaintballWorld.API.BaseModels;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Owner.Models
{
    public class MyFieldsResponseModel : ResponseBase
    {
        public FieldId FieldId { get; set; }
    }
}
