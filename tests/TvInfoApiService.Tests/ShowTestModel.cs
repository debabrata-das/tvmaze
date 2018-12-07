using System;
using System.Collections.Generic;
using TvInfo.Persistence.Models;

namespace TvInfoApiService.Tests
{
    internal class ShowTestModel : IEquatable<ShowTestModel>, IEquatable<Show>
    {
        public Show CreateShow()
        {
            return new Show
            {
                Id = Id,
                Name = Name,
                Cast = Cast
            };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Cast> Cast { get; } = new List<Cast>();

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case ShowTestModel providerModel: return Equals(providerModel);
                case Show show: return Equals(show);
                default: return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(ShowTestModel other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Equals(Cast, other.Cast);
        }

        public bool Equals(Show other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Equals(Cast, other.Cast);
        }
    }
}