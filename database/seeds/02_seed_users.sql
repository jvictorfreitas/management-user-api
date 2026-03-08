INSERT INTO users
(
    "GuidId",
    "Name",
    "Cpf",
    "AccountStatus",
    "created_at"
)
VALUES
(gen_random_uuid(), 'João Silva', '12345678900', 1, NOW()),
(gen_random_uuid(), 'Maria Souza', '98765432100', 1, NOW()),
(gen_random_uuid(), 'Pedro Santos', '45678912300', 0, NOW());