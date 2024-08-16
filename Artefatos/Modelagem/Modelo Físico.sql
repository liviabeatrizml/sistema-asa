CREATE DATABASE AGENDAMENTO_DE_SERVICOS_ACADEMICOS;
USE AGENDAMENTO_DE_SERVICOS_ACADEMICOS;

CREATE TABLE Discente (
    id_discente INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(70),
    email VARCHAR(255) NOT NULL UNIQUE,
    senha VARCHAR(99),
    matricula INT UNIQUE,
    telefone INT NOT NULL UNIQUE,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    curso VARCHAR(100)
);

CREATE TABLE ServicoDisponivel (
    id_servico INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(100),
    descricao TEXT,
    tipoAtendimento VARCHAR(100)
);

CREATE TABLE Profissional (
    id_profissional INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(70),
    email VARCHAR(255) NOT NULL UNIQUE,
    senha VARCHAR(99),
    servico_id INT,
    FOREIGN KEY (servico_id) REFERENCES ServicoDisponivel(id_servico)
);

CREATE TABLE HorarioDisponivel (
    id_horario INT AUTO_INCREMENT PRIMARY KEY,
    horaInicio TIME,
    horaFim TIME,
    diaDaSemana VARCHAR(15),
    profissional_id INT,
    FOREIGN KEY (profissional_id) REFERENCES Profissional(id_profissional)
);

CREATE TABLE Agendamento (
    id_agendamento INT AUTO_INCREMENT PRIMARY KEY,
    data DATE,
    discente_id INT,
    profissional_id INT,
    servico_id INT,
    horario_id INT,
    FOREIGN KEY (discente_id) REFERENCES Discente(id_discente),
    FOREIGN KEY (profissional_id) REFERENCES Profissional(id_profissional),
    FOREIGN KEY (servico_id) REFERENCES ServicoDisponivel(id_servico),
    FOREIGN KEY (horario_id) REFERENCES HorarioDisponivel(id_horario)
);


CREATE TABLE RelatorioServico (
    id_relatorio INT AUTO_INCREMENT PRIMARY KEY,
    diagnostico TEXT,
    status VARCHAR(50),
    agendamento_id INT,
    FOREIGN KEY (agendamento_id) REFERENCES Agendamento(id_agendamento)
);
