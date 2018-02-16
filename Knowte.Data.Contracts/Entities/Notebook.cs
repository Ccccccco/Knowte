﻿using SQLite;
using System;

namespace Knowte.Data.Contracts.Entities
{
    public class Notebook
    {
        [PrimaryKey()]
        public string Id { get; set; }

        public string Title { get; set; }

        public string CollectionId { get; set; }

        public long CreationDate { get; set; }

        public Notebook()
        {
        }

        public Notebook(string title)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Title = title;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.Id.Equals(((Notebook)obj).Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}