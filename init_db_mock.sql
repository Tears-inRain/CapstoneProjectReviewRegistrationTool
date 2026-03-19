-- init_db_mock.sql
-- Xóa dữ liệu (theo thứ tự Child -> Parent)
DELETE FROM SlotTopics;
DELETE FROM SlotLecturers;
DELETE FROM Slots;
DELETE FROM Topics;
DELETE FROM Students;
DELETE FROM Teams;
DELETE FROM Lecturers;
DELETE FROM ReviewRounds;

-- Reset Identity (để tự tăng ID lại từ 1)
DBCC CHECKIDENT ('Lecturers', RESEED, 0);
DBCC CHECKIDENT ('Students', RESEED, 0);
DBCC CHECKIDENT ('Teams', RESEED, 0);
DBCC CHECKIDENT ('ReviewRounds', RESEED, 0);
DBCC CHECKIDENT ('Slots', RESEED, 0);
DBCC CHECKIDENT ('Topics', RESEED, 0);

-- INSERT MOCK DATA
-- Lecturers (2 Leturers)
INSERT INTO Lecturers (FullName, Email, MinSlot, MaxSlot) VALUES 
('Nguyen Van A', 'gv1@fpt.edu.vn', 1, 10),
('Tran Thi B', 'gv2@fpt.edu.vn', 1, 10);

-- Topics (5 Topics supervised randomly)
INSERT INTO Topics (Title, Description, LecturerId) VALUES 
('AI Chatbot', 'Description 1', 1),
('E-commerce Web', 'Description 2', 2),
('Mobile App', 'Description 3', 1),
('Game Engine', 'Description 4', 2),
('IoT Smart Home', 'Description 5', 1);

-- Teams (5 Teams mapping TopicId)
INSERT INTO Teams (TeamName, LeaderId, TopicId) VALUES 
('TEAM_01', 1, 1),
('TEAM_02', 2, 2),
('TEAM_03', 3, 3),
('TEAM_04', 4, 4),
('TEAM_05', 5, 5);

-- Students (5 Students with TeamId)
INSERT INTO Students (RollNumber, FullName, Email, TeamId) VALUES 
('SE10001', 'Student 1', 'sv1@fpt.edu.vn', 1),
('SE10002', 'Student 2', 'sv2@fpt.edu.vn', 2),
('SE10003', 'Student 3', 'sv3@fpt.edu.vn', 3),
('SE10004', 'Student 4', 'sv4@fpt.edu.vn', 4),
('SE10005', 'Student 5', 'sv5@fpt.edu.vn', 5);

-- ReviewRounds (1 Round)
INSERT INTO ReviewRounds (RoundNumber, StartDate, EndDate) VALUES 
(1, '2026-05-01 08:00:00', '2026-05-15 17:00:00');

-- Slots (Mock 3 Slots empty for auto-scheduling)
INSERT INTO Slots (ReviewRound, Room, StartTime, EndTime) VALUES 
(1, 'Room 101', '2026-05-02 08:00:00', '2026-05-02 10:00:00'),
(1, 'Room 102', '2026-05-02 10:00:00', '2026-05-02 12:00:00'),
(1, 'Room 103', '2026-05-02 13:00:00', '2026-05-02 15:00:00');
