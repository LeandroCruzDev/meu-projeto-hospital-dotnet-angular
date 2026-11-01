import http from 'k6/http';
import { check, sleep } from 'k6';


export const options = {
    stages: [
        {duration: '15', target: 100},
        {duration: '30s', target: 400},
        {duration: '40s', target: 400},
        {duration: '15s', target: 0},
    ],
    thresholds: {
        http_req_duration: ['p(95)<1000'],
        http_req_failed: ['rate<0.01'],
    },
};

export default function () {
    const url = 'http://localhost:5099/api/patients';

    const randomId = Math.floor(Math.random() * 100000000)
    const payload = JSON.stringify({
        name: `Paciente Stress ${randomId}`,
        cpf: `${randomId}`.padStart(11, '0')
    });


    const params = {
        headers: {'Content-Type' : 'application/json'},
    };

    const res = http.post(url, payload, params);

    const success = check(res, {
    'Criado com sucesso (200/201)': (r) => r.status === 200 || r.status === 201,
  });

  // NOVO CÓDIGO: Imprime a causa da falha
  if (!success) {
    console.log(`FALHA | Status: ${res.status} | Resposta: ${res.body}`);
  }

    sleep(0.1);














}

