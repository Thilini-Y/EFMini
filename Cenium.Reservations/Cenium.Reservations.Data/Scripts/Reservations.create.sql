/*
 * Copyright (c) Cenium AS. All Right Reserved
 *
 * This source is subject to the Cenium License.
 * Please see the License.txt file for more information.
 * All other rights reserved.
 * 
 * http://www.cenium.com
 * 
 * Change History:
 * 
 * User        Date          Comment
 * ----------- ------------- --------------------------------------------------------------------------------------------
 * Thilini.Y   11/15/2021    Created
 */

/* Replace with component specific create script */

create table [dbo].[Reservations_Reservations] (
    [ReservationId] [bigint] not null identity,
	[PropertyContextId] [bigint] not null,
	[ReservationNumber] [bigint] not null,
	[CheckInDate] [datetime] not null,
	[CheckOutDate] [datetime] not null,
	[Status] [nvarchar](255) not null,
	[RoomId] [bigint] not null,
	[ContactId] [bigint] not null,
    [TenantId] [uniqueidentifier] not null,
    [RowVersion] [rowversion] not null,
    primary key ([ReservationId])
);


#SetVersion([Cenium.Reservations.Data.ReservationsEntitiesDbContext], [Reservations], [0.0.0.1], [D6730250496FD32AE0DA18B2B509E96F110F83848EA3C2E468E298EFF9E9BB32])
