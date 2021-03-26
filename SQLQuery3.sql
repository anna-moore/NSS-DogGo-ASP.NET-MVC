 SELECT o.Id, o.[Name], o.Email, o.[Address], o.Phone, n.Name
                        FROM Owner o
                        JOIN  Neighborhood n ON o.NeighborhoodId = n.id