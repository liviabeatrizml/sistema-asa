CREATE DATABASE asa;
USE asa;

CREATE TABLE Discente (
    id_discente INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(70) NOT NULL,
    email VARCHAR(255) NOT NULL,
    senha VARCHAR(99) NOT NULL,
    matricula INT NOT NULL,
    telefone VARCHAR(20) NOT NULL,
    curso VARCHAR(100) NOT NULL,
    salt VARCHAR(255) NOT NULL
);

CREATE TABLE HorarioDisponivel (
    id_horario INT AUTO_INCREMENT PRIMARY KEY,
    horaInicio TIME(6) NOT NULL,
    horaFim TIME(6) NOT NULL,
    diaDaSemana INT NOT NULL,
    profissional_id INT NOT NULL,
    FOREIGN KEY (profissional_id) REFERENCES Profissional(id_profissional)
);

CREATE TABLE ServicoDisponivel (
    id_servico INT AUTO_INCREMENT PRIMARY KEY,
    tipo LONGTEXT NOT NULL,
    tipoAtendimento LONGTEXT NOT NULL
);

CREATE TABLE Profissional (
    id_profissional INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(70) NOT NULL,
    email VARCHAR(255) NOT NULL,
    senha VARCHAR(99) NOT NULL,
	salt VARCHAR(255) NOT NULL,
    servico_id INT NOT NULL,
    descricao LONGTEXT NOT NULL,
    FOREIGN KEY (servico_id) REFERENCES ServicoDisponivel(id_servico)
);

CREATE TABLE Agendamento (
    id_agendamento INT AUTO_INCREMENT PRIMARY KEY,
    data DATETIME(6) NOT NULL,
    discente_id INT NOT NULL,
    profissional_id INT NOT NULL,
    servico_id INT NOT NULL,
    horario_id INT NOT NULL,
    status LONGTEXT NOT NULL,
    FOREIGN KEY (discente_id) REFERENCES Discente(id_discente),
    FOREIGN KEY (profissional_id) REFERENCES Profissional(id_profissional)
);
