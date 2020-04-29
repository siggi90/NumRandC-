using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NumRand {
	public class random {
		private List<cylinder> cylinders;
		private string state_file_path;

		public random(string state_file_path) {
			this.state_file_path = state_file_path;
			this.cylinders = new List<cylinder>();
			this.construct_cylinders(13);
			this.get_state();
		}

		public void get_state() {
			string parse_double = "";
			try {
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				StreamReader sr = new StreamReader(this.state_file_path);
				string line;

				// Read and display lines from the file until 
				// the end of the file is reached. 
				while ((line = sr.ReadLine()) != null) {
					parse_double = line;//Console.WriteLine(line);
				}
				sr.Close();
				
			} catch (Exception e) {
				// Let the user know what went wrong.
				System.Diagnostics.Debug.WriteLine("The file could not be read:");
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
			List<string> split = new List<string>(parse_double.Split(';'));
			this.cylinders[0].particle_position.a = Convert.ToDouble(split[0]);//this.n.evaluate(split[0]);
			this.cylinders[0].particle_position.b = Convert.ToDouble(split[1]);//this.n.evaluate(split[1]);
			this.cylinders[0].particle_direction =  Convert.ToDouble(split[2]);
			int counter = 3;
			while(counter-3 < this.cylinders.Count) {
				this.cylinders[counter-3].phase_offset = Convert.ToDouble(split[counter]);
				counter++;
			}
		}

		public void save_state(directional_vector final_state) {
			string save_double = "";
			save_double += final_state.vector.a.ToString()+";";
			save_double += final_state.vector.b.ToString()+";";//this.n.real_fraction_value(final_state.vector.b, "10")+";";
			save_double += final_state.direction+";";//this.n.real_fraction_value(final_state.direction, "10")+";";
			foreach(cylinder c in this.cylinders) {
				save_double += c.phase_offset+";";
			}
			try {
				using(StreamWriter sw = new StreamWriter(this.state_file_path, false)) {
					sw.WriteLine(save_double);
					sw.Close();
				}
			} catch(Exception e) {
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}
		}

		public void construct_cylinders(int count=13) {
			List<double> speed = this.prime_numbers(count);
			List<double> phase_offset = this.construct_phase_offset(count);
			speed = this.interlace(speed);
			phase_offset = this.interlace(phase_offset);

			phase_offset.Add(0);
			speed.Add(1);

			int counter = 0;
			while(counter < count) {
				this.cylinders.Add(new cylinder(phase_offset[counter], speed[counter]));
				counter++;
			}
		}

		private List<double> prime_list = new List<double> { 1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43 };

		public List<double> prime_numbers(int count) {
			/*int counter = 0;
			List<double> prime_numbers = new List<double>();
			while(prime_numbers.Count < count) {
				if(this.n.is_prime(counter.Todouble())) {
					prime_numbers.Add(count.Todouble());
				}
				counter++;
			}
			return prime_numbers;*/
			return this.prime_list;
		}

		public List<double> interlace(List<double> arr) {
			List<double> result = new List<double>();
			int counter = 0;
			int mid_point = arr.Count/2;
			while(counter <= mid_point) {
				result.Add(arr[counter]);
				if(arr.Count-counter-1 != counter && arr.Count-counter-1 > 0) {
					result.Add(arr[arr.Count-counter-1]);
				}
				counter++;
			}
			return result;
		}

		public List<double> construct_phase_offset(int count) {
			int phase = 0;
			List<double> phases = new List<double>();
			while(phases.Count < count) {
				phases.Add(phase);
				phase += 30;
				if(phase == 360) {
					phase = 0;
				}
			}
			return phases;
		}

		public directional_vector run_simulation(int index, vector particle_position=null, double? particle_direction=null) {
			if(index < this.cylinders.Count) {
				if(particle_position != null) {
					this.cylinders[index].particle_position = particle_position;
				}
				if(particle_direction != null) {
					this.cylinders[index].particle_direction = Convert.ToDouble(particle_direction);
				}
				directional_vector result = this.cylinders[index].calculate_translation();
				return this.run_simulation(++index, result.vector, result.direction);
			}
			this.cylinders[0].particle_position = particle_position;
			this.cylinders[0].particle_direction = Convert.ToDouble(particle_direction);
			//Console.WriteLine(particle_position);
			//Console.WriteLine(particle_direction);
			return new directional_vector(particle_position, Convert.ToDouble(particle_direction));
		}

		public double get_number() {
			this.save_state(this.run_simulation(0));
			double phase = this.cylinders[this.cylinders.Count-1].phase_offset;
			double number = Math.Round(phase / 36);//this.n.divide(new double(phase, "0/1"), new double("36", "0/1")).value;
			return number;
		}

		public string get_string(int length=1) {
			string result = "";
			int counter = 0;
			while(counter < length) {
				result += this.get_number().ToString();
				counter++;
			}
			return result;
		}

		public List<double> get_values(int from, int to, int amount) {
			List<double> values = new List<double>();
			int to_length = to.ToString().Length;
			while(values.Count < amount) {
				double value = Convert.ToDouble(this.get_string(to_length));
				if(value >= from && value <= to) {
					values.Add(value);
				}
			}
			return values;
		}
	}
}
