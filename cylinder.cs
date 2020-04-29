using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumRand {
	public class cylinder {
		public double speed;
		public double phase_offset;
		public vector particle_position; 
		public double particle_direction;
		public double radius;
		public vector center;


		public double radians;
		double pi;

		private vector gap;

		private double zero;

		public cylinder(double phase_offset=0, double speed=1) {
			this.radius = 50;//new double("50", "0/1");
			double a = 355;//new double("355", "0/1");
			double b = 113;//new double("113", "0/1");
			this.pi = a/b;//this.n.divide(a, b);
			//this.radians = this.n.divide(this.n.multiply(this.pi, new double("2", "0/1")), new double("360", "0/1"));
			//this.radians.remainder = this.n.shorten_fraction(this.radians.remainder);
			this.radians = this.pi*2 / 360;
			//Console.WriteLine("radians: "+this.radians);
			//this.radians = 
			this.zero = 0;//new double("0", "0/1");
			this.center = new vector(this.zero, this.zero);
			this.particle_position = new vector(this.zero, this.zero);
			this.particle_direction = 60;//new double("60", "0/1");
			this.speed = speed;//new double(speed, "0/1");
			this.gap = new vector(5, 5);
			this.phase_offset = phase_offset;
			this.calculate_rotation();
		}

		public directional_vector calculate_translation() {
			bool within_gap = this.within_gap();
			while(!within_gap) {
				double particle_direction_radians = this.particle_direction;//new double(this.particle_direction, "0/1");
				particle_direction_radians = particle_direction * this.radians;//this.n.multiply(particle_direction_radians, this.radians);
				/*trigonometry_values values = this.n.trigonometry_values_radian(particle_direction_radians);
				System.Diagnostics.Debug.WriteLine(values.cos);
				System.Diagnostics.Debug.WriteLine(values.sin);*/
				double quick_numeric = particle_direction_radians;//Convert.ToDouble(this.n.real_fraction_value(particle_direction_radians, "10"));
				double sin = Math.Sin(quick_numeric);
				double cos = Math.Cos(quick_numeric);
				double sin_value = sin;//this.n.common_value(sin.Todouble());
				double cos_value = cos; //this.n.common_value(cos.Todouble());
				//System.//Console.WriteLine("numeric: "+quick_numeric.Todouble());
				//System.//Console.WriteLine("sin_value: "+sin_value.Todouble());
				//System.//Console.WriteLine("cos_value: "+cos_value.Todouble());


				this.particle_position.a = this.particle_position.a + cos_value;
				this.particle_position.b = this.particle_position.b + sin_value;
				//this.n.shorten_fraction(this.particle_position.a);
				//this.n.shorten_fraction(this.particle_position.b);
				//this.shorten_vector(this.particle_position);
				this.within_borders();
				within_gap = this.within_gap();
				this.calculate_rotation();
			}
			//Console.WriteLine("cylinder");
			return new directional_vector(this.particle_position, this.particle_direction);
		}

		/*public void shorten_vector(vector v) {
			//System.Diagnostics.Debug.WriteLine(v.a.remainder);
			//System.Diagnostics.Debug.WriteLine(v.b.remainder);
			this.n.shorten_fraction(v.a);
			this.n.shorten_fraction(v.b);
		}*/

		public double get_direction(double degree, vector direction_coordinates) {
			double long_side_direction = this.particle_direction + degree;//this.n.add(this.particle_direction, new double(degree, "0/1"));
			////Console.WriteLine("long_side_direction: "+long_side_direction);
			vector long_side_coordinates = this.degree_to_coordinates(long_side_direction);
			////Console.WriteLine("long_side_coordinates: "+long_side_coordinates);
			long_side_coordinates = this.reset_vector_length(long_side_coordinates, this.radius);
			////Console.WriteLine("long_side_coorindates: "+long_side_coordinates);

			vector projection = this.projection(long_side_coordinates, direction_coordinates);
			////Console.WriteLine("projection: "+projection);

			vector middle_vector = this.subtract_vector(this.particle_position, projection);
			////Console.WriteLine("middle_vector: "+middle_vector);
			vector middle_sub = this.stretch_vector(long_side_coordinates, 0.5);
			////Console.WriteLine("middle_sub: "+middle_sub);

			middle_vector = this.subtract_vector(this.particle_position, middle_sub);
			////Console.WriteLine("middle_vector: "+middle_vector);
			middle_vector = this.reverse_vector(middle_vector);
			////Console.WriteLine("middle_vector: "+middle_vector);

			direction_coordinates = new vector(direction_coordinates.a, -direction_coordinates.b);
			////Console.WriteLine("direction_coordinates: "+direction_coordinates);

			vector reflection = this.reflection(this.reverse_vector(direction_coordinates), middle_vector);
			////Console.WriteLine("reflection: "+reflection);
			return this.coordinates_to_degree(reflection);
		}

		public void within_borders() {
			double distance = this.distance(this.particle_position);
			bool change_direction = false;
			bool pivot = false;
			if(distance > this.radius) { //|| this.within_pivot()) {
				change_direction = true;
			}
			if(this.within_pivot()) {
				change_direction = true;
				pivot = true;
			}
			if(change_direction) {
				if(!pivot) {
					double fraction = this.radius / distance;//this.n.divide(this.n.subtract(this.radius, new double("1", "0/1")), distance);
					this.particle_position = this.reset_distance(this.particle_position, fraction);
				}
				vector direction_coordinates = this.degree_to_coordinates(this.particle_direction);
				this.particle_direction = this.get_direction(90, direction_coordinates);
				/*this.particle_direction = this.n._add(this.particle_direction, this.get_direction("90", direction_coordinates));
				if(this.n._larger(this.particle_direction, "360")) {
					this.particle_direction = this.n._subtract(this.particle_direction, "360");
				}*/
				//this.previous_directions.Add(this.particle_direction);
				/*if(this.previous_directions.Count > 2) {
					if(this.particle_direction == this.previous_directions[this.previous_directions.Count-3] || this.n._add(this.particle_direction, "1") == this.previous_directions[this.previous_directions.Count-3] || this.n._subtract(this.particle_direction, "1") == this.previous_directions[this.previous_directions.Count-3]) {
						//Console.WriteLine("jolt");
						this.particle_direction = this.n.negative(new double(this.particle_direction, "0/1")).value;
						this.particle_position = new vector(this.zero, this.zero);
					}
				}*/
				//Console.WriteLine("---pos:");
				//Console.WriteLine(this.particle_position);
				//Console.WriteLine(this.particle_direction.ToString());
				//Console.WriteLine("change direction");
				//Console.ReadLine();
			}
			
		}

		public List<double> previous_directions = new List<double>();

		public double distance(vector v) {
			double a_squared = Math.Pow(v.a, 2);//this.n.power(v.a, new double("2", "0/1"));
			double b_squared = Math.Pow(v.b, 2);//this.n.power(v.b, new double ("2", "0/1"));

			double root = a_squared+b_squared;//this.n.add(a_squared, b_squared);
			//this.n.shorten_fraction(root);
			root = Math.Sqrt(root);//this.n.power(root, new double("0", "1/2"));
			//this.n.shorten_fraction(root);
			return root;
		}

		public double dot_product(vector a, vector b) {
			double result_a = a.a*b.a;//this.n.multiply(a.a, b.a);
			double result_b = a.b*b.b;//this.n.multiply(a.b, b.b);
			double dot_product = result_a+result_b;//this.n.add(result_a, result_b);
			//this.n.shorten_fraction(dot_product);
			return dot_product;
		}

		public vector projection(vector u, vector v) {
			double division = Math.Pow(this.distance(v), 2);//this.n.power(this.distance(v), new double("2", "0/1"));
			if(division == this.zero) {
				return new vector(this.zero, this.zero);;
			}
			vector vector = new vector(v.a, v.b);
			double multiplication = this.dot_product(u, v);
			multiplication = multiplication / division;//this.n.divide(multiplication, division);
			vector.a = vector.a * multiplication;//this.n.multiply(vector.a, multiplication);
			vector.b = vector.b * multiplication;//this.n.multiply(vector.b, multiplication);
			//this.shorten_vector(vector);
			return vector;
		}

		public bool compare_vectors(vector u, vector v) {
			if(u.a == v.a && u.a == v.b) {
				return true;
			}
			return false;
		}

		public double angle_between_vectors(vector u, vector v) {
			if(this.compare_vectors(u, v)) {
				return this.zero;
			}
			double dot = this.dot_product(u, v);
			//Console.WriteLine("dot: "+dot);
			double u_distance = this.distance(u);
			//Console.WriteLine("u_distance: "+u_distance);
			double v_distance = this.distance(v);
			//Console.WriteLine("v_distance: "+v_distance);
			double division = u_distance+v_distance;
			//Console.WriteLine("division: "+division);
			double result = dot / division;
			//Console.WriteLine("result: "+result);
			double quick_numeric = result;//Convert.ToDouble(this.n.real_fraction_value(result, "10"));
			//Console.WriteLine("quick_numeric: "+quick_numeric);
			quick_numeric = Math.Acos(quick_numeric);
			//Console.WriteLine("quick_numeric: "+quick_numeric);
			result = quick_numeric;//this.n.common_value(quick_numeric.Todouble());
			result = result * (1/this.radians);//this.n.multiply(result, this.n.divide(new double("1", "0/1"), this.radians));
			//Console.WriteLine("result: "+result);
			return result;
		}

		public vector flip_vector(vector u) {
			vector v = new vector(u.a, -u.b);
			return v;
		}

		public double degree_difference(double a, double b) {
			if(0 > a) {
				a = 360+a;//this.n._add("360", a);
			}
			if(0 > b) {
				b = 360+b;//this.n._add("360", b);
			}
			double result = a-b;//this.n._subtract(a, b);
			return Math.Abs(result);//this.n.absolute(new double(result, "0/1")).value;
		}

		public vector reset_distance(vector point, double fraction) {
			vector v = new vector(point.a, point.b);
			v.a = v.a*fraction;//this.n.multiply(v.a, fraction);
			v.b = v.b*fraction;//this.n.multiply(v.b, fraction);
			//this.shorten_vector(v);
			return v;
		}

		public vector reverse_vector(vector v) {
			vector u = new vector(-v.a, -v.b);
			return u;
		}

		public vector reset_vector_length(vector point, double length) {
			double initial_length = this.distance(point);
			////Console.WriteLine("length");
			////Console.WriteLine(length);
			////Console.WriteLine("initial_length");
			////Console.WriteLine(initial_length);
            ///
            if(initial_length == 0) {
                 initial_length = 1;
            }
			double fraction = length/initial_length;//this.n.divide(length, initial_length);//length/initial_length;
			vector new_point = this.reset_distance(point, fraction);
			//this.shorten_vector(new_point);
			return new_point;
		}

		public bool within_gap() {
			vector particle_position = this.particle_position;
			//Console.WriteLine("particle_position: "+particle_position);
			vector gap = this.gap;
			//Console.WriteLine("gap: "+this.gap);
			vector projection = this.projection(particle_position, this.gap);
			//Console.WriteLine("projection: "+projection);
			double distance = this.vector_distance(particle_position, projection);
			//Console.WriteLine("distance: "+distance);
			
			double radial_position = this.distance(this.particle_position);
			//Console.WriteLine("radial_position: "+radial_position);
			double distance_radius = 0.3;//new double("3", "0/1");
			//Console.WriteLine("distance_radius: "+distance_radius);
			if(distance_radius >= distance && radial_position >= 10 && 40 >= radial_position) {
			//if(this.n.larger(distance_radius, distance)) {
				return true;
			}
			////Console.WriteLine("distance: "+distance);
			////Console.WriteLine("radial position: "+radial_position);
			return false;
		}

		public bool within_pivot() {
			vector particle_position = this.particle_position;
			vector pivot = this.reverse_vector(this.gap);
			vector projection = this.projection(particle_position, pivot);
			double distance = this.vector_distance(particle_position, projection);

			double radial_position = this.distance(this.particle_position);
			double distance_radius = 3/10;//new double("0", "3/10");
			double pivot_range = 1; //new double("1", "0/1");
			double pivot_width = 1/2; //new double("0", "1/2");
			
			if(pivot_range >= distance && radial_position >= 10 && 40 >= radial_position) {
				return true;
			}
			return false;
		}

		public double vector_distance(vector v, vector u) {
			//Console.WriteLine("v: "+v);
			//Console.WriteLine("u: "+u);
			double a_squared = Math.Pow((u.a-v.a), 2);//this.n.power(this.n.subtract(u.a, v.a), new double("2", "0/1"));
			//Console.WriteLine("a_squared: "+a_squared);
			double b_squared = Math.Pow((u.b-v.b), 2);// this.n.power(this.n.subtract(u.b, v.b), new double ("2", "0/1"));
			//Console.WriteLine("b_squared: "+b_squared);

			double root = a_squared+b_squared;//this.n.add(a_squared, b_squared);
			//Console.WriteLine("root: "+root);
			root = Math.Sqrt(root);//this.n.power(root, new double("0", "1/2"));
			//Console.WriteLine("root: "+root);
			return root;
		}

		public void calculate_rotation() {
			double radian = this.phase_offset * this.radians;//this.n.multiply(new double(this.phase_offset, "0/1"), this.radians);
			//double quick_numeric = Convert.ToDouble(this.n.real_fraction_value(radian, "10"));
			double quick_numeric = radian;
			double cos = Math.Cos(quick_numeric);
			double sin = Math.Sin(quick_numeric);
			double x = cos * this.radius;//this.n.multiply(cos, new double("50", "0/1"));
			double y = sin * this.radius;//this.n.multiply(sin, new double("50", "0/1"));
			this.gap = new vector(x, y);
			////Console.WriteLine("gap: "+gap.a+" - "+gap.b);
			////Console.WriteLine("particle: "+this.particle_position.a+" - "+this.particle_position.b);
			this.phase_offset = this.phase_offset + this.speed;//this.n._add(this.phase_offset, this.speed.value);
			if(this.phase_offset >= 360) {
				this.phase_offset = this.phase_offset - 360;//this.n._subtract(this.phase_offset, "360");
			}
			////Console.WriteLine("phase_offset::"+this.phase_offset);
		}

		public vector subtract_vector(vector u, vector v) {
			vector result = new vector(u.a, u.b);
			result.a = result.a - v.a;//this.n.subtract(result.a, v.a);
			result.b = result.b - v.b; //this.n.subtract(result.b, v.b);
			//this.shorten_vector(result);
			return result;
		}

		public vector sum_vector(vector u, vector v) {
			vector result = new vector(u.a, u.b);
			result.a = result.a+v.a;//this.n.add(result.a, v.a);
			result.b = result.b+v.b;//this.n.add(result.b, v.b);
			//this.shorten_vector(result);
			return result;
		}

		public vector stretch_vector(vector u, double unit_value) {
			vector result = new vector(u.a, u.b);
			result.a = result.a*unit_value;//this.n.multiply(result.a, unit_value);
			result.b = result.b*unit_value;//this.n.multiply(result.b, unit_value);
			//this.shorten_vector(result);
			return result;
		}

		public double coordinates_to_degree(vector v) {
			double quick_numeric_a = v.a;//Convert.ToDouble(this.n.real_fraction_value(v.a, "10"));
			double quick_numeric_b = v.b; //Convert.ToDouble(this.n.real_fraction_value(v.b, "10"));
			double rad = Math.Atan2(quick_numeric_b, quick_numeric_a);
			double radian_value = rad; //this.n.common_value(rad.Todouble());
			double deg = 180/this.pi;// this.n.divide(new double("180", "0/1"), this.pi);
			deg = radian_value * deg;//this.n.multiply(radian_value, deg);
			return deg;
		}

		public vector degree_to_coordinates(double deg) {
			double radian = deg * this.radians;//this.n.multiply(deg, this.radians);
			double quick_numeric = radian;//Convert.ToDouble(this.n.real_fraction_value(radian, "10"));
			////Console.WriteLine("quick_numeric: "+quick_numeric);
			double x = Math.Cos(quick_numeric);//this.n.common_value(Math.Cos(quick_numeric).Todouble());
			double y = Math.Sin(quick_numeric);//this.n.common_value(Math.Sin(quick_numeric).Todouble());
			////Console.WriteLine("x: "+x);
			////Console.WriteLine("y: "+y);
			vector v = new vector(x, y);
			return v;
		}

		public vector normalize_vector(vector v) {
			double length = this.distance(v);
            if(length == 0) {
                return v;
            }
			vector vector = new vector(v.a, v.b);
			vector.a = vector.a/length;
			vector.b = vector.b/length;
			return vector;
		}

		public vector reflection(vector d, vector c) {
			////Console.WriteLine("reflection");
			////Console.WriteLine(d);
			////Console.WriteLine(c);
			vector n = new vector(c.a, c.b);
			n = this.normalize_vector(n);
			//Console.WriteLine(n);
			double dot = this.dot_product(d, n);
			dot = dot * 2;//new double("2", "0/1");
			////Console.WriteLine(dot);
			vector stretch = this.stretch_vector(n, dot);
			////Console.WriteLine(stretch);
			vector subtract = this.subtract_vector(d, stretch);
			////Console.WriteLine(subtract);
			return subtract;
		}
	}

	public class vector {
		public double a;
		public double b;

		public vector(double a, double b) {
			this.a = a;
			this.b = b;
		}

		public override string ToString() {
			return Convert.ToString(this.a)+"-"+Convert.ToString(this.b);
		}
	}

	public class directional_vector {
		public vector vector;
		public double direction;

		public directional_vector(vector vector, double direction) {
			this.vector = vector;
			this.direction = direction;
		}
	}

}
