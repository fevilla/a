using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;
using System.Linq;
using System.Threading.Tasks;
using static StressProblems;
using TMPro;
using static StressProblems;

public class PlayerWriteSendOpenAI : MonoBehaviour
{
    [SerializeField] Button sendButton;
    [SerializeField] private TMPro.TMP_InputField playerWriteInputField;
    [SerializeField] private TMPro.TextMeshPro stoneText;
    [SerializeField] GameObject[] stars;
    [SerializeField] GameObject stone;
    private int idx = 0;
    //private string[] motivaciones = {"Felicidades por tu logro" , "Ese es el camino no te detengas" , "Sigue asi , lo estas haciendo genial" , "Cada paso cuenta , y tu estas avanzando , ¡Muy bien!"};
    private OpenAIApi openai = new OpenAIApi();

    void Start()
    {
        sendButton.GetComponent<Button>().onClick.AddListener(Send);
        PlayerWriteSendOpenAI[] scripts = FindObjectsOfType<PlayerWriteSendOpenAI>();
        foreach (PlayerWriteSendOpenAI script in scripts)
        {
            Debug.Log(script.gameObject.name, script.gameObject);
        }
    }

    async void Send() {
        if (StressProblems.currentProblemIdx >= StressProblems.problems.Length) return;
        string problem = StressProblems.problems[StressProblems.currentProblemIdx];

        Debug.Log("StressProblems.currentProblemIdx: " + StressProblems.currentProblemIdx);

        // string solution = "organizare el poco tiempo que me queda para estudiar y dare mi examen con lo que aprenda sabiendo que mas adelante me organizare mejor y dare un mejor examen";
        // string solution = "me voy a poner a preguntar a la gente si debo estresarme o no";
        string solution = playerWriteInputField.text;
        
<<<<<<< HEAD
        //string[] responses = {await Ask(problem, solution), await Ask(problem, solution)};
        // var decision = responses.Aggregate(0, (result, response) => {
        //     if (
        //         (response[0] == 'N' ||
        //         response[0] == 'n') &&
        //         response[1] == 'o' 
        //     ) {
        //        return result - 1;
        //     } else if (
        //         (response[0] == 'S' ||
        //         response[0] == 's') &&
        //         (response[1] == 'i' ||
        //         response[1] == 'í') 
        //     ) {
        //        return result + 1;
        //     }
		// 	return 0;
		// });

        var decision  = 2;

        if (decision == 0) {
            var response = await Ask(problem, solution);
            decision = (response[0] == 'S' || response[0] == 's') ? 1 : -1;
        }

        if (decision > 0) {
=======
        string[] responses = await Task.WhenAll(
            Enumerable.Repeat(
                Ask(problem, solution),
                7
            ).ToArray()
        );
        var decision = responses.Aggregate(0, (result, response) => {
            if (
                (response[0] == 'N' ||
                response[0] == 'n') &&
                response[1] == 'o' 
            ) {
               return result - 1;
            } else if (
                (response[0] == 'S' ||
                response[0] == 's') &&
                (response[1] == 'i' ||
                response[1] == 'í') 
            ) {
               return result + 1;
            }
			return 0;
		});

        if (decision >= 5) {
            // star.gameObject.SetActive(true);
>>>>>>> 1ac6f008980ad0cbf79a1e59fa417cd3bf18c557
            playerWriteInputField.text = "";
            stoneText.text = "";

            StressProblems.currentProblemIdx++;

            if (StressProblems.currentProblemIdx >= StressProblems.problems.Length) return;
            problem = StressProblems.problems[StressProblems.currentProblemIdx];
            stoneText.text = problem;
            stone.gameObject.SetActive(false);
            stars[idx].transform.position = stone.transform.position;
            stars[idx].gameObject.SetActive(true);
            Vector3 newPosition = stone.transform.position;
            newPosition.x += 20.0f; 
            newPosition.y = 7.9f;
            Debug.Log("Posicion stone" + newPosition.x);
            stone.transform.position = newPosition; 
            idx++;
            //star.GetComponentInChildren<TextMeshPro>().text = motivaciones[idx++];
        } else {
            playerWriteInputField.text = "Podrías mejorar tu respuesta...";
        }

        Debug.Log("Decision (if >0 then yes): " + decision);

    }

    void Update() {

        // Debug.Log(star.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
        // Debug.Log(star.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("star_animation"));
        // if (
        //     star.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("star_animation") &&
        //     star.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f
        // ) {
        //     star.gameObject.SetActive(false);
        // }
    }

    async Task<string> Ask (string problem, string solution) {
        string result;
        var req = new CreateChatCompletionRequest {
            Model = "gpt-3.5-turbo",
            Messages = new List<ChatMessage>()
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = 
                        "Hola, resume en un \"sí\" o un \"no\" si la siguiente frase \"" + solution +
                        "\" " + "alivia el estrés de esta frase \"" + problem + "\".",              
                }
            }
        };
        var completionResponse = await openai.CreateChatCompletion(req);
        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0) {
            var message = completionResponse.Choices[0].Message;
            message.Content = message.Content.Trim();
            
            result = message.Content;
        } else {
            result = "Text wasn't generated from this prompt";
        }

        return result;
    }
}
