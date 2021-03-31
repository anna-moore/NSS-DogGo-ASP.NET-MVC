﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DogGo.Models;
using System;
using System.Collections.Generic;


namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }


        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT w.Id, w.[Date], w.Duration,
                                            w.DogId, w.WalkerId,
                                            Walker.Name as Walker, d.Name as Dog, 
                                            o.Name as Owner
                                        FROM Walks w
                                        JOIN Dog d on w.DogId = d.Id
                                        JOIN Owner o on d.OwnerId = o.Id
                                        JOIN Walker on w.WalkerId = Walker.Id";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Walk> walks = new List<Walk>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Name = reader.GetString(reader.GetOrdinal("Owner")),
                        };

                        Walk walk = new Walk
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Owner = owner
                        };

                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
            }
        }

        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                       SELECT wk.Id, w.Id AS WalkerId, d.Id AS DogId, w.[Name], wk.[Date], wk.Duration, o.[Name] AS Owner
                                        FROM Walker w
                                        LEFT JOIN Walks wk on wk.WalkerId = w.Id
                                        LEFT JOIN Dog d on wk.DogId = d.Id
                                        LEFT JOIN [Owner] o on o.Id = d.OwnerId
                                        WHERE w.id = @id
                                        ORDER BY o.name ASC
                                       ";
                    cmd.Parameters.AddWithValue("@id", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();

                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Name = reader.GetString(reader.GetOrdinal("Owner")),
                        };
                        Walk walk = new Walk
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")) / 60,
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Owner = owner
                        };

                        walks.Add(walk);
                    }

                    reader.Close();
                    return walks;
                }
            }
        }
    }
}
