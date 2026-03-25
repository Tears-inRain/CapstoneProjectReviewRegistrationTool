-- init_db_mock.sql
-- Xóa dữ liệu (theo thứ tự Child -> Parent)
DELETE FROM SlotTopics;
DELETE FROM SlotLecturers;
DELETE FROM Slots;
DELETE FROM Students;
DELETE FROM Teams;
DELETE FROM Topics;
DELETE FROM Lecturers;

-- INSERT MOCK DATA WITH EXPLICIT IDs

-- Lecturers (2 Leturers)
SET IDENTITY_INSERT Lecturers ON;
INSERT INTO Lecturers (Id, FullName, Email, MinSlot, MaxSlot) VALUES 
(1, 'Nguyen Van A', 'gv1@fpt.edu.vn', 1, 10),
(2, 'Tran Thi B', 'gv2@fpt.edu.vn', 1, 10);
SET IDENTITY_INSERT Lecturers OFF;

-- Topics (5 Topics supervised randomly)
SET IDENTITY_INSERT Topics ON;
INSERT INTO Topics (Id, Title, Description, LecturerId) VALUES 
(1, 'AI Chatbot', 'Description 1', 1),
(2, 'E-commerce Web', 'Description 2', 2),
(3, 'Mobile App', 'Description 3', 1),
(4, 'Game Engine', 'Description 4', 2),
(5, 'IoT Smart Home', 'Description 5', 1);
SET IDENTITY_INSERT Topics OFF;

-- Teams (5 Teams mapping TopicId)
SET IDENTITY_INSERT Teams ON;
INSERT INTO Teams (Id, TeamName, LeaderId, TopicId) VALUES 
(1, 'TEAM_01', 1, 1),
(2, 'TEAM_02', 2, 2),
(3, 'TEAM_03', 3, 3),
(4, 'TEAM_04', 4, 4),
(5, 'TEAM_05', 5, 5);
SET IDENTITY_INSERT Teams OFF;

-- Students (5 Students with TeamId)
SET IDENTITY_INSERT Students ON;
INSERT INTO Students (Id, RollNumber, FullName, Email, TeamId) VALUES 
(1, 'SE10001', 'Student 1', 'sv1@fpt.edu.vn', 1),
(2, 'SE10002', 'Student 2', 'sv2@fpt.edu.vn', 2),
(3, 'SE10003', 'Student 3', 'sv3@fpt.edu.vn', 3),
(4, 'SE10004', 'Student 4', 'sv4@fpt.edu.vn', 4),
(5, 'SE10005', 'Student 5', 'sv5@fpt.edu.vn', 5);
SET IDENTITY_INSERT Students OFF;

-- Slots (Mock 3 Slots empty for auto-scheduling)
SET IDENTITY_INSERT Slots ON;
INSERT INTO Slots (Id, ReviewRound, Room, StartTime, EndTime, RegistrationDeadline) VALUES 
(1, 1, 'Room 101', '2026-05-02 08:00:00', '2026-05-02 10:00:00', '2026-04-30 00:00:00'),
(2, 1, 'Room 102', '2026-05-02 10:00:00', '2026-05-02 12:00:00', '2026-04-30 00:00:00'),
(3, 1, 'Room 103', '2026-05-02 13:00:00', '2026-05-02 15:00:00', '2026-04-30 00:00:00');
SET IDENTITY_INSERT Slots OFF;
